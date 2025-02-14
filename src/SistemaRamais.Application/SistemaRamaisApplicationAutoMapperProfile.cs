using System;
using SistemaRamais.Shared;
using Volo.Abp.AutoMapper;
using SistemaRamais.Ramais;
using AutoMapper;

namespace SistemaRamais;

public class SistemaRamaisApplicationAutoMapperProfile : Profile
{
    public SistemaRamaisApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Ramal, RamalDto>();
        CreateMap<Ramal, RamalExcelDto>();
    }
}