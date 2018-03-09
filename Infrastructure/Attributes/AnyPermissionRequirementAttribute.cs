// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Attributes
{
    // TODO constructeur qui prend juste un [] de string "Security.Admin", "Security.Write" par exemple et implémenter le contrôle.
    // Plus simple de lecture pour celui qui doit utiliser l'attribut.
    public class AnyPermissionRequirementAttribute : ActionFilterAttribute
    {
        private readonly Enums.Permission[] _permissions;
        private readonly string[] _extensionAssemblySimpleNames;

         /// <summary>
         /// Allows access when the user has at least one of the claims of type "Permission" with value
         /// defined by its level (Admin, Write, Read...) and its scope (Security, ExtensionX...).
         /// </summary>
         /// <param name="permissionCode_"></param>
         /// <param name="extensionAssemblySimpleName_"></param>
        public AnyPermissionRequirementAttribute(Enums.Permission[] permissionCode_, string[] extensionAssemblySimpleName_)
         {
             _permissions = permissionCode_;
             _extensionAssemblySimpleNames = extensionAssemblySimpleName_;
         }

        public override void OnActionExecuting(ActionExecutingContext context_)
        {
            bool accessGranted = false;
            IEnumerable<string> userPermissionClaimValues = context_.HttpContext.User.Claims.Where(c_ => c_.Type == Enums.ClaimType.Permission.ToString()).Select(c_ => c_.Value);
            for (int i = 0; i < _permissions.Length; i++)
            {
                string claimValue =
                    Util.GetScopedPermissionIdentifier(_permissions[i], _extensionAssemblySimpleNames[i]);
                if (userPermissionClaimValues.Contains(claimValue))
                {
                    // access granted
                    accessGranted = true;
                    break;
                }
            }

            if (!accessGranted)
            {
                context_.Result = new ForbidResult();
            }
        }
    }
}