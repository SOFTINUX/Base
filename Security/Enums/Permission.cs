namespace Security.Enums
{
    public static class Permission
    {
        public static string EditUser = "edit_user";
        public static string EditRole = "edit_role";
        public static string EditGroup = "edit_group";
        public static string EditPermission = "edit_permission";

        /// <summary>
        /// ID of PermissionLevel records
        /// </summary>
        public enum PermissionLevelId
        {
            IdNever = 1,
            IdNo = 2,
            IdReadOnly = 3,
            IdReadWrite = 4
        }

        /// <summary>
        /// Value of PermissionLevel records
        /// </summary>
        public enum PermissionLevelValue
        {
            Never = 1,
            No = 2,
            ReadOnly = 4,
            ReadWrite = 8
        }

        /// <summary>
        /// ID of Permission records
        /// </summary>
        public enum PermissionId
        {
            EditUser = 1,
            EditRole = 2,
            EditGroup = 3,
            EditPermission = 4
        }
    }
}
