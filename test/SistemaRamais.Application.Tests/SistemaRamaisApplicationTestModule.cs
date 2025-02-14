using Volo.Abp.Modularity;

namespace SistemaRamais;

[DependsOn(
    typeof(SistemaRamaisApplicationModule),
    typeof(SistemaRamaisDomainTestModule)
)]
public class SistemaRamaisApplicationTestModule : AbpModule
{

}
