using Volo.Abp.Modularity;

namespace SistemaRamais;

/* Inherit from this class for your domain layer tests. */
public abstract class SistemaRamaisDomainTestBase<TStartupModule> : SistemaRamaisTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
