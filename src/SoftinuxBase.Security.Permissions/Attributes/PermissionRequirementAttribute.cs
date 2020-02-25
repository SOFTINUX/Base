// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SoftinuxBase.Security.Permissions.Enums;

namespace SoftinuxBase.Security.Permissions.Attributes
{
    [Obsolete]
    public class PermissionRequirementAttribute : ActionFilterAttribute
    {
        private readonly Permission _permissionLevel;
        private readonly string _extensionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRequirementAttribute"/> class.
        /// Allows access when the user has the permission : a claim of type "Permission" with value
        /// defined by its level (Admin, Write, Read...) and an extension name (SoftinuxBase.Security, ProjectX.ExtensionY...).
        /// Or the user has a permission with same extension name but higher level (Admin when Write is the minimum requested to be granted access).
        /// </summary>
        /// <param name="permissionLevel_">Permission level.</param>
        /// <param name="extensionAssemblySimpleName_">the simple name of the assembly extension.</param>
        public PermissionRequirementAttribute(Permission permissionLevel_, string extensionAssemblySimpleName_)
        {
            if (string.IsNullOrWhiteSpace(extensionAssemblySimpleName_))
            {
                throw new ArgumentNullException(nameof(extensionAssemblySimpleName_), "Please specify the extension name");
            }

            _permissionLevel = permissionLevel_;
            _extensionName = extensionAssemblySimpleName_;
        }

        /// <summary>
        /// Gets permission unique identifier.
        /// </summary>
        public string PermissionIdentifier => PermissionHelper.GetExtensionPermissionIdentifier(_permissionLevel, _extensionName);

        /// <summary>
        /// TODO document.
        /// </summary>
        /// <param name="context_">ActionExecutingContext.</param>
        public override void OnActionExecuting(ActionExecutingContext context_)
        {
            bool accessGranted = false;

            // Get the user claim, if any, matching the extension of interest
            Claim claimOfLookupExtension = context_.HttpContext.User.Claims.FirstOrDefault(c_ => c_.Type == ClaimType.Permission && c_.Value.ToString().StartsWith($"{_extensionName}."));

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
