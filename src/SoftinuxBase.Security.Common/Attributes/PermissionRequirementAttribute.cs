// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SoftinuxBase.Security.Common.Enums;

namespace SoftinuxBase.Security.Common.Attributes
{
    public class PermissionRequirementAttribute : ActionFilterAttribute
    {
        private readonly Permission _permissionLevel;
        private readonly string _extensionName;

        /// <summary>
        /// Allows access when the user has the permission : a claim of type "Permission" with value
        /// defined by its level (Admin, Write, Read...) and an extension name (SoftinuxBase.Security, ProjectX.ExtensionY...).
        /// Or the user has a permission with same extension name but higher level (Admin when Write is the minimum requested to be granted access).
        /// Default extension name (assembly simple name) is "SoftinuxBase.Security".
        /// </summary>
        public PermissionRequirementAttribute(Permission permissionName_, string extensionAssemblySimpleName_ = "SoftinuxBase.Security")
        {
            _permissionLevel = permissionName_;
            _extensionName = extensionAssemblySimpleName_;
        }

        /// <summary>
        /// Gets permission unique identifier.
        /// </summary>
        public string PermissionIdentifier => PermissionHelper.GetExtensionPermissionIdentifier(_permissionLevel, _extensionName);

        public override void OnActionExecuting(ActionExecutingContext context_)
        {
            bool accessGranted = false;

            // Get the user claim, if any, matching the extension of interest
            Claim claimOfLookupExtension = context_.HttpContext.User.Claims.FirstOrDefault(c_ => c_.Type == ClaimType.Permission.ToString() && c_.Value.ToString().StartsWith($"{_extensionName}."));

            if (claimOfLookupExtension != null)
            {
                Permission currentLevel = Enum.Parse<Permission>(PermissionHelper.GetPermissionLevel(claimOfLookupExtension));
                if ((int)currentLevel >= (int)_permissionLevel)
                {
                    // access granted
                    accessGranted = true;
                }
            }

            if (!accessGranted)
            {
                context_.Result = new ForbidResult();
            }
        }
    }
}
