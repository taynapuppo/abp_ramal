using SistemaRamais.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace SistemaRamais.Web.Pages;

public abstract class SistemaRamaisPageModel : AbpPageModel
{
    protected SistemaRamaisPageModel()
    {
        LocalizationResourceType = typeof(SistemaRamaisResource);
    }
}
