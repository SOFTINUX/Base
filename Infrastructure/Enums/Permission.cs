// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace Infrastructure.Enums
{
    /// <summary>
    /// This extendable (because it is partial) class defines all the permissions "codes",
    /// i.e. distinct values in column "Code" of table "Permission".
    /// </summary>
    public partial class Permissions
    {
        // TODO replace these by "Admin", "Write", "Read", "Never"

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

    }
}
