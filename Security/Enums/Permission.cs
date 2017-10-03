namespace Security.Enums
{
    public static class Permission
    {
        public static string EditUser = "edit_user";
        public static string EditRole = "edit_role";
        public static string EditGroup = "edit_group";
        public static string EditPermission = "edit_permission";

        public enum PermissionLevelId
        {
            Never = 1,
            No = 2,
            ReadOnly = 3,
            ReadWrite = 4
        }

        public enum PermissionId
        {
            EditUser = 1,
            EditRole = 2,
            EditGroup = 3,
            EditPermission = 4
        }
    }
}
