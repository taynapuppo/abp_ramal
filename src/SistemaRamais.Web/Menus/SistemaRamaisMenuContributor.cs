using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using SistemaRamais.Localization;
using SistemaRamais.Permissions;
using SistemaRamais.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.AuditLogging.Web.Navigation;
using Volo.Abp.LanguageManagement.Navigation;
using Volo.Abp.TextTemplateManagement.Web.Navigation;
using Volo.Abp.OpenIddict.Pro.Web.Menus;
using Volo.Saas.Host.Navigation;

namespace SistemaRamais.Web.Menus;

public class SistemaRamaisMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<SistemaRamaisResource>();

        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                SistemaRamaisMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );

        //HostDashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                SistemaRamaisMenus.HostDashboard,
                l["Menu:Dashboard"],
                "~/HostDashboard",
                icon: "fa fa-line-chart",
                order: 2
            ).RequirePermissions(SistemaRamaisPermissions.Dashboard.Host)
        );

        //TenantDashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                SistemaRamaisMenus.TenantDashboard,
                l["Menu:Dashboard"],
                "~/Dashboard",
                icon: "fa fa-line-chart",
                order: 2
            ).RequirePermissions(SistemaRamaisPermissions.Dashboard.Tenant)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                SistemaRamaisMenus.Ramais,
                l["Menu:Ramais"],
                url: "/Ramais",
                icon: "fa fa-phone",
                order: 3,
                requiredPermissionName: SistemaRamaisPermissions.Ramais.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                SistemaRamaisMenus.Relatorios,
                l["Menu:Relatorios"], 
                url: "/Relatorios",
                icon: "fa fa-file-alt",
                order: 4
            )
        );

        // Aba de Gráficos
        context.Menu.AddItem(
            new ApplicationMenuItem(
                SistemaRamaisMenus.Graficos,
                l["Menu:Graficos"],
                url: "/Graficos",
                icon: "fa fa-chart-bar",
                order: 5
            )
        );

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;

        //Administration->Saas
        administration.SetSubItemOrder(SaasHostMenuNames.GroupName, 1);

        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);

        //Administration->OpenIddict
        administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 3);

        //Administration->Language Management
        administration.SetSubItemOrder(LanguageManagementMenuNames.GroupName, 4);

        //Administration->Text Template Management
        administration.SetSubItemOrder(TextTemplateManagementMainMenuNames.GroupName, 5);

        //Administration->Audit Logs
        administration.SetSubItemOrder(AbpAuditLoggingMainMenuNames.GroupName, 6);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 7);

       
        return Task.CompletedTask;
    }
}