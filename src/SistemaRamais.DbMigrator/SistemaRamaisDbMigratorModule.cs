using SistemaRamais.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace SistemaRamais.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SistemaRamaisEntityFrameworkCoreModule),
    typeof(SistemaRamaisApplicationContractsModule)
)]
public class SistemaRamaisDbMigratorModule : AbpModule
{
}
