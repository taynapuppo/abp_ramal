using Volo.Abp.Modularity;

namespace SistemaRamais;

[DependsOn(
    typeof(SistemaRamaisDomainModule),
    typeof(SistemaRamaisTestBaseModule)
)]
public class SistemaRamaisDomainTestModule : AbpModule
{

}
