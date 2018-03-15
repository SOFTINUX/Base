// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Attributes
{

    public class PermissionRequirementAttribute : ActionFilterAttribute
    {
        private Enums.Permission _permissionName;
        private string _scope;

        /// <summary>
        /// Permission unique identifier.
        /// </summary>
        public string PermissionIdentifier { get { return Util.GetScopedPermissionIdentifier(_permissionName, _scope); } }

        /// <summary>
        /// Allows access when the user has the permission : a claim of type "Permission" with value
        /// defined by its level (Admin, Write, Read...) and its scope (Security, ExtensionX...).
        /// Or the user has a permission with same scope but higher level (Admin when Write is the minimum requested to be granted access).
        /// </summary>
        public PermissionRequirementAttribute(Enums.Permission permissionName_, string extensionAssemblySimpleName_)
        {
            _permissionName = permissionName_;
            _scope = extensionAssemblySimpleName_;
        }

        public override void OnActionExecuting(ActionExecutingContext context_)
        {
            bool accessGranted = false;
            // Get the user claim, if any, matching the scope of interest
            Claim claimOfLookupScope = context_.HttpContext.User.Claims.Where(c_ => c_.Type == Enums.ClaimType.Permission.ToString() && c_.Value.ToString().StartsWith($"{_scope}.")).FirstOrDefault();

            if(claimOfLookupScope != null)
            {
                Enums.Permission currentLevel = Enum.Parse<Enums.Permission>(Util.GetPermissionLevel(claimOfLookupScope));
                if((int)currentLevel >= (int)_permissionName)
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
