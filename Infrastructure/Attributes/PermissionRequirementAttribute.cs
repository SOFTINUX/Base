// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace Infrastructure.Attributes
{
    public class PermissionRequirementAttribute : ClaimRequirementAttribute
    {
        /// <summary>
        /// Permission unique identifier.
        /// </summary>
        public string PermissionIdentifier { get; }

        public PermissionRequirementAttribute(Enums.Permission permissionCode_, string extensionAssemblySimpleName_)
        : base(Enums.ClaimType.Permission, Util.GetScopedPermissionIdentifier(permissionCode_, extensionAssemblySimpleName_))
        {
            PermissionIdentifier =
                Util.GetScopedPermissionIdentifier(permissionCode_, extensionAssemblySimpleName_);
        }
    }
}
