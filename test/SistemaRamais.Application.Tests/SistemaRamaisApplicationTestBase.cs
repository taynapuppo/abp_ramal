using Volo.Abp.Modularity;

namespace SistemaRamais;

public abstract class SistemaRamaisApplicationTestBase<TStartupModule> : SistemaRamaisTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
