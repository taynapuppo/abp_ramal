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
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;


namespace SistemaRamais.Ramais
{
    [Authorize(SistemaRamaisPermissions.Ramais.Default)]
    public abstract class RamaisAppServiceBase : SistemaRamaisAppService
    {
        protected readonly IDistributedCache<RamalDownloadTokenCacheItem, string> _downloadTokenCache;
        protected readonly IRamalRepository _ramalRepository;
        protected readonly RamalManager _ramalManager;
        protected readonly ILookupNormalizer _lookupNormalizer;
        protected readonly IIdentityUserRepository _userRepository;



        // Construtor com dependências
        public RamaisAppServiceBase(
            IRamalRepository ramalRepository,
            RamalManager ramalManager,
            IDistributedCache<RamalDownloadTokenCacheItem, string> downloadTokenCache,
            ILookupNormalizer lookupNormalizer, IIdentityUserRepository userRepository
        )
        {
            _downloadTokenCache = downloadTokenCache;
            _ramalRepository = ramalRepository;
            _ramalManager = ramalManager;
            _lookupNormalizer = lookupNormalizer;
            _userRepository = userRepository;

        }
            
        public virtual async Task<PagedResultDto<RamalDto>> GetListAsync(GetRamaisInput input)
        {
            var totalCount = await _ramalRepository.GetCountAsync(_lookupNormalizer.NormalizeName(input.FilterText), _lookupNormalizer.NormalizeName(input.Nome), input.Numero, _lookupNormalizer.NormalizeName(input.Departamento), _lookupNormalizer.NormalizeEmail(input.Email));
            var items = await _ramalRepository.GetListAsync(_lookupNormalizer.NormalizeName(input.FilterText), _lookupNormalizer.NormalizeName(input.Nome), input.Numero, _lookupNormalizer.NormalizeName(input.Departamento), _lookupNormalizer.NormalizeEmail(input.Email), input.Sorting, input.MaxResultCount, input.SkipCount);

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
                _lookupNormalizer.NormalizeEmail(input.Email),
                _lookupNormalizer.NormalizeName(input.Departamento)
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
                _lookupNormalizer.NormalizeName(input.Departamento),
                input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Ramal, RamalDto>(ramal);
        }


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