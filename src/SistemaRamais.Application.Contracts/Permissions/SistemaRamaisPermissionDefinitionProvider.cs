using SistemaRamais.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace SistemaRamais.Permissions;

public class SistemaRamaisPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SistemaRamaisPermissions.GroupName);

        myGroup.AddPermission(SistemaRamaisPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(SistemaRamaisPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(SistemaRamaisPermissions.MyPermission1, L("Permission:MyPermission1"));

        var ramalPermission = myGroup.AddPermission(SistemaRamaisPermissions.Ramais.Default, L("Permission:Ramais"));
        ramalPermission.AddChild(SistemaRamaisPermissions.Ramais.Create, L("Permission:Create"));
        ramalPermission.AddChild(SistemaRamaisPermissions.Ramais.Edit, L("Permission:Edit"));
        ramalPermission.AddChild(SistemaRamaisPermissions.Ramais.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SistemaRamaisResource>(name);
    }
}