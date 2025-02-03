using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.AuditLogging;
using Volo.Abp.Gdpr;
using Volo.Abp.LanguageManagement;
using Volo.Abp.OpenIddict;
using Volo.Abp.TextTemplateManagement;
using Volo.Saas.Host;
using Microsoft.Extensions.DependencyInjection;
using SistemaRamais.Ramais;
using Microsoft.AspNetCore.Identity;

namespace SistemaRamais;

[DependsOn(
    typeof(SistemaRamaisDomainModule),
    typeof(SistemaRamaisApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountPublicApplicationModule),
    typeof(AbpAccountAdminApplicationModule),
    typeof(SaasHostApplicationModule),
    typeof(AbpAuditLoggingApplicationModule),
    typeof(TextTemplateManagementApplicationModule),
    typeof(AbpOpenIddictProApplicationModule),
    typeof(LanguageManagementApplicationModule),
    typeof(AbpGdprApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
)]
public class SistemaRamaisApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        context.Services.AddTransient<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        
        context.Services.AddTransient<RamalManager>();

        context.Services.AddTransient<RamaisAppServiceBase, RamaisAppService>();


        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<SistemaRamaisApplicationModule>();
        });
    }
}
