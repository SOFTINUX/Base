// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Security.Data.Abstractions;

namespace SeedDatabase.Controllers
{
    [Route("dev/seed")]
    public class SeedDatabaseController : Controller
    {
        private readonly UserManager<Security.Data.Entities.User> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly IStorage _storage;

        public SeedDatabaseController(UserManager<Security.Data.Entities.User> userManager,
            RoleManager<IdentityRole<string>> roleManager,
            IStorage storage)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _storage = storage;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Hello world!");
        }

        [HttpPost]
        [Route("/dev/seed/CreateUser")]
        public async Task<IActionResult> CreateUser()
        {
            // Get the list of the role from the enum
            Role[] roles = (Role[])Enum.GetValues(typeof(Role));

            foreach(var r in roles)
            {
                // create an identity role object out of the enum value
                IdentityRole<string> identityRole = new IdentityRole<string>
                {
                        Id = r.GetRoleName(),
                        Name = r.GetRoleName()
                    };

                    if(!await _roleManager.RoleExistsAsync(roleName: identityRole.Name))
                    {
                        var result = await _roleManager.CreateAsync(identityRole);

                        //return 500 if fail
                        if(!result.Succeeded)
                            return  StatusCode(StatusCodes.Status500InternalServerError);
                    }
            }

            // our default user
            Security.Data.Entities.User user = new Security.Data.Entities.User {
                FirstName = "Doe",
                LastName = "John",
                Email = "johndoe@softinux.com",
                UserName = "johndoe@softinux.com",
                LockoutEnabled = false
            };

            // add the user to the database if it doesn't already exist
            if (await _userManager.FindByEmailAsync(user.Email) == null)
            {
                // WARNING: Do Not check in credentials of any kind into source control
                var result = await _userManager.CreateAsync(user, password: "123_Password");

                if (!result.Succeeded) //return 500 if it fails
                    return StatusCode(StatusCodes.Status500InternalServerError);

                //Assign all roles to the default user
                result = await _userManager.AddToRolesAsync(user, new[] { "Administrator" });

                if (!result.Succeeded) // return 500 if fails
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }

            SavePermissions(user);

            return Ok("User Creation Ok.");
        }

        private IActionResult SavePermissions(Security.Data.Entities.User user_)
        {
            if (user_ == null)
                return StatusCode(StatusCodes.Status204NoContent, $"User is null.");

            //IPermissionRepository repo = _storage.GetRepository<IPermissionRepository>();
            Infrastructure.Enums.Permission[] permissions = (Infrastructure.Enums.Permission[])Enum.GetValues(typeof(Permission));

            foreach(var p in permissions)
            {
                // create a permission object out of the enum value
                Security.Data.Entities.Permission permission = new Security.Data.Entities.Permission()
                {
                    Id = p.GetPermissionName(),
                    Name = p.GetPermissionName(),
                    OriginExtension = "Security"
                };

                _storage.GetRepository<IPermissionRepository>().Create(permission);
            }

            try
            {
                _storage.Save();
                SaveUserPermission("Admin", user_);
                return Ok("Saving permissions ok.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"Cannot saving permissions. Error: {e.Message}");
            }
        }

        private IActionResult SaveUserPermission(string permissionId_, Security.Data.Entities.User user_)
        {
            if (
                (!string.IsNullOrEmpty(permissionId_) || !string.IsNullOrWhiteSpace(permissionId_)) &&
                user_ != null
                )
            {
                Security.Data.Entities.UserPermission userPermission = new Security.Data.Entities.UserPermission()
                {
                    UserId = user_.Id,
                    PermissionId = permissionId_
                };

                _storage.GetRepository<IUserPermissionRepository>().Create(userPermission);
            }
            try
            {
                _storage.Save();
                SaveRolePermission("Administrator", "Admin");
                return Ok("Saving user permissions ok.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"Cannot saving user permissions. Error: {e.Message}");
            }
        }

        private IActionResult SaveRolePermission(string roleId_, string permissionId_)
        {
            if (
                (!string.IsNullOrEmpty(permissionId_) || !string.IsNullOrWhiteSpace(permissionId_)) &&
                (!string.IsNullOrEmpty(roleId_) || !string.IsNullOrWhiteSpace(roleId_))
                )
            {
                Security.Data.Entities.RolePermission rolePermission = new Security.Data.Entities.RolePermission()
                {
                    RoleId = roleId_,
                    PermissionId = permissionId_
                };
                _storage.GetRepository<IRolePermissionRepository>().Create(rolePermission);
            }

            try
            {
                _storage.Save();
                return Ok("Saving role permissions ok.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"Cannot saving role permissions. Error: {e.Message}");
            }
        }

    }
}
