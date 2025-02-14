namespace SistemaRamais.Permissions;

public static class SistemaRamaisPermissions
{
    public const string GroupName = "SistemaRamais";

    public static class Dashboard
    {
        public const string DashboardGroup = GroupName + ".Dashboard";
        public const string Host = DashboardGroup + ".Host";
        public const string Tenant = DashboardGroup + ".Tenant";
    }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class Ramais
    {
        public const string Default = GroupName + ".Ramais";
        public const string View = Default + ".View";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}