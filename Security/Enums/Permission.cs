// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace Security.Enums
{
    public static class Permission
    {
        /// <summary>
        /// Code of a permission provided by the Security extension.
        /// </summary>
        public const string PERM_CODE_EDIT_USER = "edit_user";
        /// <summary>
        /// Code of a permission provided by the Security extension.
        /// </summary>
        public const string PERM_CODE_EDIT_ROLE = "edit_role";
        /// <summary>
        /// Code of a permission provided by the Security extension.
        /// </summary>
        public const string PERM_CODE_EDIT_GROUP = "edit_group";
        /// <summary>
        /// Code of a permission provided by the Security extension.
        /// </summary>
        public const string PERM_CODE_EDIT_PERMISSION = "edit_permission";

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
