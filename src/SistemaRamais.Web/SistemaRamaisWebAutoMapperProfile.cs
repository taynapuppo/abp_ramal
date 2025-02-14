using SistemaRamais.Web.Pages.Ramais;
using Volo.Abp.AutoMapper;
using SistemaRamais.Ramais;
using AutoMapper;

namespace SistemaRamais.Web;

public class SistemaRamaisWebAutoMapperProfile : Profile
{
    public SistemaRamaisWebAutoMapperProfile()
    {
        //Define your object mappings here, for the Web project

        CreateMap<RamalDto, RamalUpdateViewModel>();
        CreateMap<RamalUpdateViewModel, RamalUpdateDto>();
        CreateMap<RamalCreateViewModel, RamalCreateDto>();
    }
}