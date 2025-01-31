using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using SistemaRamais.Permissions;
using SistemaRamais.Ramais;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text;
using FuzzySharp;




namespace SistemaRamais.Ramais
{
    [Authorize(SistemaRamaisPermissions.Ramais.Default)]
    public abstract class RamaisAppServiceBase : SistemaRamaisAppService
    {
        protected readonly IDistributedCache<RamalDownloadTokenCacheItem, string> _downloadTokenCache;
        protected readonly IRamalRepository _ramalRepository;
        protected readonly RamalManager _ramalManager;
        protected readonly ILookupNormalizer _lookupNormalizer;


        // Construtor com dependências
        public RamaisAppServiceBase(
            IRamalRepository ramalRepository,
            RamalManager ramalManager,
            IDistributedCache<RamalDownloadTokenCacheItem, string> downloadTokenCache,
            ILookupNormalizer lookupNormalizer
        )
        {
            _downloadTokenCache = downloadTokenCache;
            _ramalRepository = ramalRepository;
            _ramalManager = ramalManager;
            _lookupNormalizer = lookupNormalizer;
        }

        [AllowAnonymous]
        public virtual async Task<List<RamalDto>> GetNomeFuzzyFromDb(string query)
        {
            var normalizedQuery = NormalizeString(_lookupNormalizer.NormalizeName(query));

            // Busca todos os ramais do repositório
            var ramais = await _ramalRepository.GetListAsync();

            // Aplica a comparação fuzzy nos nomes e filtra por pontuação
            var ramaisComFuzzy = ramais
                .Select(ramal => new
                {
                    Ramal = ramal,
                    Score = Fuzz.WeightedRatio(NormalizeString(ramal.Nome), normalizedQuery)
                })
                .Where(r => r.Score > 65)
                .OrderByDescending(r => r.Score)
                .Take(10)
                .ToList();

            // Mapeia os resultados para o DTO e retorna
            return ObjectMapper.Map<List<Ramal>, List<RamalDto>>(ramaisComFuzzy.Select(r => r.Ramal).ToList());
        }


        private int GetBestFuzzyScore(string fullName, string query)
        {
            var normalizedQuery = NormalizeString(query);
            var words = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var scores = words.Select(word => Fuzz.WeightedRatio(NormalizeString(word), normalizedQuery)).ToList();

            return scores.Any() ? scores.Max() : 0;
        }


        private static string NormalizeString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // Remove acentos e converte para minúsculas
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var chr in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(chr);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(chr);
            }

            // Retorna a string normalizada e em minúsculas
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }


        public virtual async Task<PagedResultDto<RamalDto>> GetListAsync(GetRamaisInput input)
        {
            // Normaliza a entrada antes da busca
            var nomeNormalizado = string.IsNullOrEmpty(input.Nome) ? null : NormalizeString(_lookupNormalizer.NormalizeName(input.Nome));

            // Buscando com o fuzzy direto no banco
            List<RamalDto> ramaisDto = new List<RamalDto>();

            if (!string.IsNullOrEmpty(input.FilterText))
            {
                // Realiza a busca fuzzy
                var ramais = await GetNomeFuzzyFromDb(input.FilterText);
                ramaisDto = ramais;
            }
            else
            {
                // Caso contrário, faz a busca tradicional no repositório
                var ramais = await _ramalRepository.GetListAsync(
                    filterText: input.FilterText?.ToLower(),
                    nome: nomeNormalizado,
                    numero: input.Numero,
                    departamento: input.Departamento,
                    email: input.Email,
                    sorting: input.Sorting,
                    maxResultCount: input.MaxResultCount,
                    skipCount: input.SkipCount
                );

                ramaisDto = ObjectMapper.Map<List<Ramal>, List<RamalDto>>(ramais);
            }

            // Retorna os resultados com paginação
            return new PagedResultDto<RamalDto>
            {
                TotalCount = ramaisDto.Count,
                Items = ramaisDto
            };
        }

        public virtual async Task<RamalDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Ramal, RamalDto>(await _ramalRepository.GetAsync(id));
        }

        [Authorize(SistemaRamaisPermissions.Ramais.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _ramalRepository.DeleteAsync(id);
        }

        [Authorize(SistemaRamaisPermissions.Ramais.Create)]
        public virtual async Task<RamalDto> CreateAsync(RamalCreateDto input)
        {
            var ramal = await _ramalManager.CreateAsync(
                input.Nome, input.Numero, input.Departamento, input.Email,
                _lookupNormalizer.NormalizeName(input.Nome),
                _lookupNormalizer.NormalizeEmail(input.Email)
            );

            return ObjectMapper.Map<Ramal, RamalDto>(ramal);
        }

        [Authorize(SistemaRamaisPermissions.Ramais.Edit)]
        public virtual async Task<RamalDto> UpdateAsync(Guid id, RamalUpdateDto input)
        {
            var ramal = await _ramalManager.UpdateAsync(
                id,
                input.Nome, input.Numero, input.Departamento, input.Email,
                _lookupNormalizer.NormalizeName(input.Nome),
                _lookupNormalizer.NormalizeEmail(input.Email),
                input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Ramal, RamalDto>(ramal);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(RamalExcelDownloadDto input)
        {
            var downloadToken = await _downloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _ramalRepository.GetListAsync(input.FilterText, input.Nome, input.Numero, input.Departamento, input.Email);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Ramal>, List<RamalExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Ramais.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [Authorize(SistemaRamaisPermissions.Ramais.Delete)]
        public virtual async Task DeleteByIdsAsync(List<Guid> ramalIds)
        {
            await _ramalRepository.DeleteManyAsync(ramalIds);
        }

        public virtual async Task<SistemaRamais.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _downloadTokenCache.SetAsync(
                token,
                new RamalDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new SistemaRamais.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}
