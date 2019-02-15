// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SoftinuxBase.Security.Common.Enums;

namespace SoftinuxBase.Security.Common.Attributes
{
    public class AnyPermissionRequirementAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Permissions grouped by extension.
        /// </summary>
        /// <returns></returns>
        private readonly Dictionary<string, List<Permission>> _permissionsByExtension = new Dictionary<string, List<Permission>>();

        /// <summary>
        /// Allows access when the user has at least one of the claims of type "Permission" with value
        /// defined by its level (Admin, Write, Read...) and an extension name (SoftinuxBase.Security, ProjectX.ExtensionY...).
        /// </summary>
        /// <param name="permissionLevel_"></param>
        /// <param name="extensionAssemblySimpleName_"></param>
        public AnyPermissionRequirementAttribute(Permission[] permissionLevel_, string[] extensionAssemblySimpleName_)
        {
            for (int i = 0; i < permissionLevel_.Length; i++)
            {
                if (_permissionsByExtension.ContainsKey(extensionAssemblySimpleName_[i]))
                {
                    _permissionsByExtension[extensionAssemblySimpleName_[i]].Add(permissionLevel_[i]);
                }
                else
                {
                    _permissionsByExtension.Add(extensionAssemblySimpleName_[i], new List<Permission> { permissionLevel_[i] });
                }
            }
        }

        /// <summary>
        /// Allows access when the user has at least one of the claims of type "Permission" with value
        /// defined by its level (Admin, Write, Read...) and an extension name (SoftinuxBase.Security, ProjectX.ExtensionY...).
        /// </summary>
        /// <param name="permissionsForExtensions_">Values with format ExtensionName.Permission.</param>
        public AnyPermissionRequirementAttribute(string[] permissionsForExtensions_)
        {
            foreach (string perm in permissionsForExtensions_)
            {
                string extension = PermissionHelper.GetExtensionName(perm);
                Permission permission = Enum.Parse<Permission>(PermissionHelper.GetPermissionLevel(perm));

                if (_permissionsByExtension.ContainsKey(extension))
                {
                    _permissionsByExtension[extension].Add(permission);
                }
                else
                {
                    _permissionsByExtension.Add(extension, new List<Permission> { permission });
                }
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context_)
        {
            bool accessGranted = false;
            foreach (string extension in _permissionsByExtension.Keys)
            {
                // Get the user claim, if any, matching the extension of interest
                Claim claimOfLookupExtension = context_.HttpContext.User.Claims.FirstOrDefault(c_ => c_.Type == ClaimType.Permission.ToString() && c_.Value.ToString().StartsWith($"{extension}."));

                if (claimOfLookupExtension != null)
                {
                    Permission currentLevel = Enum.Parse<Permission>(PermissionHelper.GetPermissionLevel(claimOfLookupExtension));
                    foreach (var permission in _permissionsByExtension[extension])
                    {
                        var minLevel = (int)permission;
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