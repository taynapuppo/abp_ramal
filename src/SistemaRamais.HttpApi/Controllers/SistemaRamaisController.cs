using SistemaRamais.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SistemaRamais.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SistemaRamaisController : AbpControllerBase
{
    protected SistemaRamaisController()
    {
        LocalizationResource = typeof(SistemaRamaisResource);
    }
}
