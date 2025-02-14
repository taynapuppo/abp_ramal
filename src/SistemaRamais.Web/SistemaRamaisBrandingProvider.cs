using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using SistemaRamais.Localization;

namespace SistemaRamais.Web;

[Dependency(ReplaceServices = true)]
public class SistemaRamaisBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<SistemaRamaisResource> _localizer;

    public SistemaRamaisBrandingProvider(IStringLocalizer<SistemaRamaisResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
