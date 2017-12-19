namespace SeedDatabase
{
    public enum Role
    {
        Administrator,
        user,
        anonymous
    }

    public enum Group
    {
        Administrator,
        Users,
        Anonymous
    }

    public enum Permission
    {
        Admin,
        Write,
        Read
    }

    public static class Constants
    {
        public static string GetRoleName(this Role role) // convenience method
        {
            return role.ToString();
        }
        public static string GetGroupName(this Group group) // convenience method
        {
            return group.ToString();
        }
        public static string GetPermissionName(this Permission permission) // convenience method
        {
            return permission.ToString();
        }
    }
}