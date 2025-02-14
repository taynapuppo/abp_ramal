using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using SistemaRamais.Shared;

namespace SistemaRamais.Ramais
{

    public partial interface IRamaisAppService : IApplicationService
    {

        Task<PagedResultDto<RamalDto>> GetListAsync(GetRamaisInput input);

        Task<RamalDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<RamalDto> CreateAsync(RamalCreateDto input);

        Task<RamalDto> UpdateAsync(Guid id, RamalUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(RamalExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> ramalIds);

        Task<SistemaRamais.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}