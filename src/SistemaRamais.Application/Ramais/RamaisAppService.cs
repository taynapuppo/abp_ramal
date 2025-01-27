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
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace SistemaRamais.Ramais
{
    [Authorize(SistemaRamaisPermissions.Ramais.Default)]
    public abstract class RamaisAppServiceBase : SistemaRamaisAppService
    {
        protected readonly IDistributedCache<RamalDownloadTokenCacheItem, string> _downloadTokenCache;
        protected readonly IRamalRepository _ramalRepository;
        protected readonly RamalManager _ramalManager;
        protected readonly RamalSearchService _ramalSearchService;  // Corrigido para 'readonly'

        // Construtor com dependências
        public RamaisAppServiceBase(
            IRamalRepository ramalRepository,
            RamalManager ramalManager,
            IDistributedCache<RamalDownloadTokenCacheItem, string> downloadTokenCache,
            RamalSearchService ramalSearchService // Injeção de dependência para o RamalSearchService
        )
        {
            _downloadTokenCache = downloadTokenCache;
            _ramalRepository = ramalRepository;
            _ramalManager = ramalManager;
            _ramalSearchService = ramalSearchService; // Inicializando a dependência
        }

        // Método de busca fuzzy - alterado para assíncrono
        [AllowAnonymous]
        public virtual async Task<List<(string Ramal, string Responsavel, int Score)>> BuscarResponsavelFuzzy(string query)
        {
            return await _ramalSearchService.BuscarResponsavelAsync(query); // Chamada assíncrona
        }

        public virtual async Task<PagedResultDto<RamalDto>> GetListAsync(GetRamaisInput input)
        {
            var totalCount = await _ramalRepository.GetCountAsync(input.FilterText, input.Numero, input.Departamento, input.Responsavel, input.Email, input.Telefone);
            var items = await _ramalRepository.GetListAsync(input.FilterText, input.Numero, input.Departamento, input.Responsavel, input.Email, input.Telefone, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<RamalDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Ramal>, List<RamalDto>>(items)
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
                input.Numero, input.Departamento, input.Responsavel, input.Email, input.Telefone
            );

            return ObjectMapper.Map<Ramal, RamalDto>(ramal);
        }

        [Authorize(SistemaRamaisPermissions.Ramais.Edit)]
        public virtual async Task<RamalDto> UpdateAsync(Guid id, RamalUpdateDto input)
        {
            var ramal = await _ramalManager.UpdateAsync(
                id,
                input.Numero, input.Departamento, input.Responsavel, input.Email, input.Telefone, input.ConcurrencyStamp
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

            var items = await _ramalRepository.GetListAsync(input.FilterText, input.Numero, input.Departamento, input.Responsavel, input.Email, input.Telefone);

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

        [Authorize(SistemaRamaisPermissions.Ramais.Delete)]
        public virtual async Task DeleteAllAsync(GetRamaisInput input)
        {
            await _ramalRepository.DeleteAllAsync(input.FilterText, input.Numero, input.Departamento, input.Responsavel, input.Email, input.Telefone);
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
