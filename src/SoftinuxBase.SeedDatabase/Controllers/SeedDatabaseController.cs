// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using Permission = SoftinuxBase.Security.Common.Enums.Permission;

namespace SoftinuxBase.SeedDatabase.Controllers
{
    [Route("dev/seed")]
    public class SeedDatabaseController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly IStorage _storage;
        protected ILogger _logger;

        private List<IdentityRole<string>> _createdRoles = new List<IdentityRole<string>>();

        public SeedDatabaseController(UserManager<User> userManager_,
            RoleManager<IdentityRole<string>> roleManager_, ILoggerFactory loggerFactory_,
            IStorage storage_)
        {
            _userManager = userManager_;
            _roleManager = roleManager_;
            _storage = storage_;
            _logger = loggerFactory_?.CreateLogger(GetType().FullName);
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Access REST API \"Hello world!\"");
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
                    // Automatic ID
                    Name = r.GetRoleName()
                };
                _createdRoles.Add(identityRole);

                if(!await _roleManager.RoleExistsAsync(identityRole.Name))
                {
                    var result = await _roleManager.CreateAsync(identityRole);

                    //return 500 if fail
                    if(!result.Succeeded)
                    {
                        _logger.LogCritical("\"(CreateAsync) Error creating role: { @Name }\"", identityRole.Name);
                        return  StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }

            // our default user
            User johnUser = new User {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@softinux.com",
                UserName = "johndoe",
                LockoutEnabled = false
            };

            // second user
            User janeUser = new User {
                FirstName = "Jane",
                LastName = "Fonda",
                Email = "janefonda@softinux.com",
                UserName = "janefonda",
                LockoutEnabled = false
            };

            // third user
            User paulUser = new User {
                FirstName = "Paul",
                LastName = "Keller",
                Email = "paulkeller@softinux.com",
                UserName = "paulkeller",
                LockoutEnabled = false
            };

            bool firstUser = true;
            foreach (var user in new[] {johnUser, janeUser, paulUser})
            {
                // add the user to the database if it doesn't already exist
                if (await _userManager.FindByEmailAsync(user.Email) == null)
                {
                    // WARNING: Do Not check in credentials of any kind into source control
                    var result = await _userManager.CreateAsync(user, password: "123_Password");

                    if (!result.Succeeded) //return 500 if it fails
                    {
                        _logger.LogCritical("\"(CreateAsync) Error creating user: { @userEmail }\"", user.Email);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }

                    // Assign roles to user. John has Admin role, Jane and Paul have User role
                    result = await _userManager.AddToRolesAsync(user, new[] { firstUser ? "Administrator" : "User" });

                    if (!result.Succeeded) // return 500 if fails
                    {
                        _logger.LogCritical("\"(AddToRolesAsync) Error adding user to role, user: { @userEmail }, role: Administrator\"", user.Email );
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                firstUser = false;
            }
            // Save PERMISSIONS
            var saveResult = SavePermissions();
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // Save USER-PERMISSION

            // John (admin user): Admin (globally)
            saveResult = SaveUserPermission(Permission.Admin.ToString(), johnUser);
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // Paul : Admin (Chinook)
            saveResult = SaveUserPermission(Permission.Admin.ToString(), paulUser, "Chinook");
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // Save ROLE-PERMISSION
            var adminRoleId = _createdRoles.FirstOrDefault(r => r.Name == Role.Administrator.ToString()).Id;
            var userRoleId = _createdRoles.FirstOrDefault(r => r.Name == Role.User.ToString()).Id;
            var anonymousRoleId = _createdRoles.FirstOrDefault(r => r.Name == Role.Anonymous.ToString()).Id;
           
            // 1. Admin role: admin (globally)
            saveResult = SaveRolePermission(adminRoleId, Permission.Admin.ToString());
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // 2. Admin role: admin (Chinook)
            saveResult = SaveRolePermission(adminRoleId, Permission.Admin.ToString(), "Chinook");
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // 3. User role: write (globally)
            saveResult = SaveRolePermission(userRoleId, Permission.Write.ToString());
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // 4. Anonymous role: read (globally)
            saveResult = SaveRolePermission(anonymousRoleId, Permission.Read.ToString());
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            return Ok("User Creation Ok.");
        }

        private IActionResult SavePermissions()
        {
            Permission[] permissions = (Permission[])Enum.GetValues(typeof(Permission));

            foreach(var p in permissions)
            {
                // create a permission object out of the enum value
                SoftinuxBase.Security.Data.Entities.Permission permission = new SoftinuxBase.Security.Data.Entities.Permission()
                {
                    Id = p.GetPermissionName(),
                    Name = p.GetPermissionName()
                };

                _storage.GetRepository<IPermissionRepository>().Create(permission);
            }

            // Permission for Chinook administration
            SoftinuxBase.Security.Data.Entities.Permission permission1 = new SoftinuxBase.Security.Data.Entities.Permission()
            {
                Id = "Chinook.Admin", // OriginExtension + Name
                Name = "Admin"
            };

            _storage.GetRepository<IPermissionRepository>().Create(permission1);

            try
            {
                _storage.Save();

                _logger.LogInformation("\"Saving permissions ok.\"");
                return Ok($"Saving permissions {permission1.Name} ok.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("\"Error saving permission (@permission): {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", permission1.Name, e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot save permissions. Error: {e.Message}");
            }
        }

        private IActionResult SaveUserPermission(string permissionId_, User user_, string scope_ = null)
        {
            if (!string.IsNullOrWhiteSpace(permissionId_) && user_ != null)
            {
                UserPermission userPermission = new UserPermission()
                {
                    UserId = user_.Id,
                    PermissionId = permissionId_
                };
                if(!string.IsNullOrWhiteSpace(scope_))
                    userPermission.Scope = scope_;

                _storage.GetRepository<IUserPermissionRepository>().Create(userPermission);
            }
            try
            {
                _storage.Save();

                _logger.LogInformation($"\"Saving user-permission {permissionId_} ok.\"");
                return Ok("Saving user-permission ok.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("\"Error saving user-permission (@userPermission): {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", permissionId_, e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot save user-permission. Error: {e.Message}");
            }
        }

        private IActionResult SaveRolePermission(string roleId_, string permission_, string scope_ = null)
        {
            if (
                (!string.IsNullOrWhiteSpace(permission_)) &&
                (!string.IsNullOrWhiteSpace(roleId_))
                )
            {
                RolePermission rolePermission = new RolePermission()
                {
                    RoleId = roleId_,
                    PermissionId = permission_,
                };
                if(!string.IsNullOrWhiteSpace(scope_))
                    rolePermission.Scope = scope_;

                _storage.GetRepository<IRolePermissionRepository>().Create(rolePermission);
            }

            try
            {
                _storage.Save();
                _logger.LogInformation($"\"Saving role-permission: {permission_}, to role: {roleId_}, with scope: {scope_ ?? "SoftinuxBase.Security"} ok.\"");
                return Ok("Saving role-permission ok.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("\"Error saving role-permission (@rolePermission, @permissionId): {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", roleId_, permission_, e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot save role-permission. Error: {e.Message}");
            }
        }

    }
}
