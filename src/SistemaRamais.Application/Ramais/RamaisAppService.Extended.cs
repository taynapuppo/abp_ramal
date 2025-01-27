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
using SistemaRamais.Shared;

namespace SistemaRamais.Ramais
{
    public class RamaisAppService : RamaisAppServiceBase, IRamaisAppService
    {
        public RamaisAppService(
            IRamalRepository ramalRepository,
            RamalManager ramalManager,
            IDistributedCache<RamalDownloadTokenCacheItem, string> downloadTokenCache,
            RamalSearchService ramalSearchService 
        )
            : base(ramalRepository, ramalManager, downloadTokenCache, ramalSearchService) 
        {
        }
    }
}