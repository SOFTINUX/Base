// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;

namespace SoftinuxBase.Security.Permissions.Enums
{
    public enum Permissions : short
    {
        NotSet = 0, //error condition

        [Display(GroupName = "User management", Name = "CanList", Description = "Can list users")]
        ListUsers = 10,

        [Display(GroupName = "User management", Name = "CanRead", Description = "Can read users details")]
        ReadUsers = 11,

        [Display(GroupName = "User management", Name = "CanCreateUsers", Description = "Can create users")]
        CreateUsers = 12,

        [Display(GroupName = "User management", Name = "CanEdit", Description = "Can edit users")]
        EditUsers = 13,

        [Display(GroupName = "User management", Name = "CanEditUsersPermissions", Description = "Can edit users' permissions")]
        EditUsersPermissions = 14,

        [Display(GroupName = "User management", Name = "CanDelete", Description = "Can delete users")]
        DeleteUsers = 15,

        [Display(GroupName = "Role management", Name = "CanList", Description = "Can list roles")]
        ListRoles = 20,

        [Display(GroupName = "Role management", Name = "CanRead", Description = "Can read roles")]
        ReadRoles = 21,

        [Display(GroupName = "Role management", Name = "CanCreate", Description = "Can create roles")]
        CreateRoles = 22,

        [Display(GroupName = "Role management", Name = "CanEdit", Description = "Can edit roles")]
        EditRoles = 23,

        [Display(GroupName = "Role management", Name = "CanDelete", Description = "Can delete roles")]
        DeleteRoles = 24,

        [Display(GroupName = "General management", Name = "CanAccessExtension", Description = "Can access extension")]
        AccessExtension = 30,

        [Display(GroupName = "General management", Name = "CanRead", Description = "Can read")]
        Read = 31,

        [Display(GroupName = "General management", Name = "CanCreate", Description = "Can create")]
        Create = 32,

        [Display(GroupName = "General management", Name = "CanEdit", Description = "Can edit")]
        Edit = 33,

        [Display(GroupName = "General management", Name = "CanDelete", Description = "Can delete")]
        Delete = 34,

        [Display(GroupName = "General management", Name = "CanAdmin", Description = "Can admin")]
        Admin = 35,

        /*       //Here is an example of very detailed control over something

               [Display(GroupName = "CacheTest", Name = "Cache1", Description = "Base permission to update permission test")]
               Cache1 = 60,
               [Display(GroupName = "CacheTest", Name = "Cache2", Description = "Permission to toggle for update permission test")]
               Cache2 = 61,

               [Display(GroupName = "Impersonation", Name = "Impersonate - straight", Description = "Impersonate user using their permissions")]
               Impersonate = 70,
               [Display(GroupName = "Impersonation", Name = "Impersonate - enhanced", Description = "Impersonate user using current permissions")]
               ImpersonateKeepOwnPermissions = 71,

               //This is an example of what to do with permission you don't used anymore.
               //You don't want its number to be reused as it could cause problems 
               //Just mark it as obsolete and the PermissionDisplay code won't show it
               [Obsolete]
               [Display(GroupName = "Old", Name = "Not used", Description = "example of old permission")]
               OldPermissionNotUsed = 100,

               //This is an example of a permission linked to a optional (paid for?) feature
               //The code that turns roles to permissions can
               //remove this permission if the user isn't allowed to access this feature
               [LinkedToModule(PaidForModules.Feature1)]
               [Display(GroupName = "Features", Name = "Feature1", Description = "Can access feature1")]
               Feature1Access = 1000,
               [LinkedToModule(PaidForModules.Feature2)]
               [Display(GroupName = "Features", Name = "Feature2", Description = "Can access feature2")]
               Feature2Access = 1001,
               */

        // Should not be managed in administration interface
        AccessAll = Int16.MaxValue,
    }
}