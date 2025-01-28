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


namespace SistemaRamais.Ramais
{
    [Authorize(SistemaRamaisPermissions.Ramais.Default)]
    public abstract class RamaisAppServiceBase : SistemaRamaisAppService
    {
        protected readonly IDistributedCache<RamalDownloadTokenCacheItem, string> _downloadTokenCache;
        protected readonly IRamalRepository _ramalRepository;
        protected readonly RamalManager _ramalManager;
        protected readonly IRamalSearchService _ramalSearchService; 
        protected readonly ILookupNormalizer _lookupNormalizer;

        // Construtor com dependências
        public RamaisAppServiceBase(
            IRamalRepository ramalRepository,
            RamalManager ramalManager,
            IDistributedCache<RamalDownloadTokenCacheItem, string> downloadTokenCache,
            IRamalSearchService ramalSearchService, 
            ILookupNormalizer lookupNormalizer
        )
        {
            _downloadTokenCache = downloadTokenCache;
            _ramalRepository = ramalRepository;
            _ramalManager = ramalManager;
            _ramalSearchService = ramalSearchService; 
            _lookupNormalizer = lookupNormalizer;
        }

        // Método de busca fuzzy - alterado para assíncrono
        [AllowAnonymous]
        public virtual async Task<List<(string Ramal, string Nome, int Score)>> BuscarNomeFuzzy(string query)
        {
            return await _ramalSearchService.BuscarNomeAsync(query);  // Chamando o serviço de busca usando a interface
        }

        public virtual async Task<PagedResultDto<RamalDto>> GetListAsync(GetRamaisInput input)
        {
            var totalCount = await _ramalRepository.GetCountAsync(input.FilterText, input.Nome, input.Numero, input.Departamento, input.Email);
            var items = await _ramalRepository.GetListAsync(input.FilterText, input.Nome, input.Numero, input.Departamento, input.Email, input.Sorting, input.MaxResultCount, input.SkipCount);

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
