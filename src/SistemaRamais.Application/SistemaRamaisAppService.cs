using SistemaRamais.Localization;
using Volo.Abp.Application.Services;

namespace SistemaRamais;

/* Inherit your application services from this class.
 */
public abstract class SistemaRamaisAppService : ApplicationService
{
    protected SistemaRamaisAppService()
    {
        LocalizationResource = typeof(SistemaRamaisResource);
    }
}
