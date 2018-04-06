// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Security.Common.Enums;

namespace Security.Common.Attributes
{
    public class AnyPermissionRequirementAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Permissions grouped by scope.
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<string, List<Permission>> _scopedPermissions = new Dictionary<string, List<Permission>>();

        /// <summary>
        /// Allows access when the user has at least one of the claims of type "Permission" with value
        /// defined by its level (Admin, Write, Read...) and its scope (Security, ExtensionX...).
        /// </summary>
        /// <param name="permissionCode_"></param>
        /// <param name="extensionAssemblySimpleName_"></param>
        public AnyPermissionRequirementAttribute(Permission[] permissionCode_, string[] extensionAssemblySimpleName_)
        {
            for (int i = 0; i < permissionCode_.Length; i++)
            {
                if (_scopedPermissions.ContainsKey(extensionAssemblySimpleName_[i]))
                    _scopedPermissions[extensionAssemblySimpleName_[i]].Add(permissionCode_[i]);
                else
                    _scopedPermissions.Add(extensionAssemblySimpleName_[i], new List<Permission> { permissionCode_[i] });
            }
        }

        /// <summary>
        /// Allows access when the user has at least one of the claims of type "Permission" with value
        /// defined by its level (Admin, Write, Read...) and its scope (Security, ExtensionX...).
        /// </summary>
        /// <param name="scopedPermissions_">Values with format ExtensionName.Permission</param>
        public AnyPermissionRequirementAttribute(string[] scopedPermissions_)
        {
            foreach (string perm in scopedPermissions_)
            {
                string scope = PermissionHelper.GetPermissionScope(perm);
                Permission permission = Enum.Parse<Permission>(PermissionHelper.GetPermissionLevel(perm));

                if (_scopedPermissions.ContainsKey(scope))
                    _scopedPermissions[scope].Add(permission);
                else
                    _scopedPermissions.Add(scope, new List<Permission> { permission });
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context_)
        {
            bool accessGranted = false;
            foreach (string scope in _scopedPermissions.Keys)
            {
                // Get the user claim, if any, matching the scope of interest
                Claim claimOfLookupScope = context_.HttpContext.User.Claims.FirstOrDefault(c_ => c_.Type == ClaimType.Permission.ToString() && c_.Value.ToString().StartsWith($"{scope}."));

                if (claimOfLookupScope != null)
                {
                    Permission currentLevel = Enum.Parse<Permission>(PermissionHelper.GetPermissionLevel(claimOfLookupScope));
                    foreach (int minLevel in _scopedPermissions[scope])
                    {
                        if ((int)currentLevel >= minLevel)
                        {
                            // access granted
                            accessGranted = true;
                            break;
                        }
                    }
                }
            }

            if (!accessGranted)
            {
                context_.Result = new ForbidResult();
            }
        }
    }
}