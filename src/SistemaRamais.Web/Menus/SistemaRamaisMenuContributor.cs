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
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Users;
using Microsoft.AspNetCore.Mvc;

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

        if (context.ServiceProvider.GetRequiredService<ICurrentUser>().IsInRole("UsersDefault"))
        {
            context.Menu.Items.Clear(); 
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    "RamaisSearch",
                    l["Menu:Ramais"],
                    url: "/Ramais",
                    icon: "fa fa-phone",
                    order: 1
                )
            );
            return Task.CompletedTask;
        }


        // Remove o item Home para todos os outros usuários
        context.Menu.AddItem(
            new ApplicationMenuItem(
                SistemaRamaisMenus.Ramais,
                l["Menu:Ramais"],
                url: "/Ramais",
                icon: "fa fa-phone",
                order: 1,
                requiredPermissionName: SistemaRamaisPermissions.Ramais.Default)
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