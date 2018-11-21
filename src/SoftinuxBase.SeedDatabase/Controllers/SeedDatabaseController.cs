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
using System.Net;
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
        private readonly ILogger _logger;

        private readonly List<IdentityRole<string>> _createdRoles = new List<IdentityRole<string>>();
        // 0: Never, 1: Read, 2: Write, 3 : Admin
        private readonly List<Security.Data.Entities.Permission> _createdPermissions = new List<Security.Data.Entities.Permission>();
        // 0: John, 1: Jane, 2: Paul
        private User[] _createdUsers = new User[3];

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
            var saveRolesResult = await SaveRoles();
            if (saveRolesResult.StatusCode != (int) HttpStatusCode.OK)
            {
                // Failure. Return error 500.
                return saveRolesResult;
            }

            var saveUsersResult = await SaveUsers();
            if (saveUsersResult.StatusCode != (int) HttpStatusCode.OK)
            {
                // Failure. Return error 500 with message.
                return saveUsersResult;
            }

            // Save PERMISSIONS
            var savePermissionsResult = SavePermissions();
            if (savePermissionsResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return StatusCode(500, savePermissionsResult);

            // Save USER-PERMISSION
            var saveUsersPermissionsResult = SaveUserPermission();
            if (saveUsersPermissionsResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return StatusCode(500, saveUsersPermissionsResult);

            // Save ROLE-PERMISSION
            var saveRolesPermissionsResult = SaveRolePermission();
            if (saveRolesPermissionsResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return StatusCode(500, saveRolesPermissionsResult);

            return Ok(
                saveRolesResult.Value.ToString() +
                saveUsersResult.Value.ToString() +
                savePermissionsResult +
                saveUsersPermissionsResult +
                saveRolesPermissionsResult +
                "Demo database initialization Ok.");
        }

        private ObjectResult SaveRolePermission()
        {
            var adminRoleId = _createdRoles.FirstOrDefault(r_ => r_.Name == Role.Administrator.ToString())?.Id;
            var userRoleId = _createdRoles.FirstOrDefault(r_ => r_.Name == Role.User.ToString())?.Id;
            var anonymousRoleId = _createdRoles.FirstOrDefault(r_ => r_.Name == Role.Anonymous.ToString())?.Id;

            var adminPermissionId = _createdPermissions.FirstOrDefault(p_ => p_.Name == Permission.Admin.ToString())?.Id;
            var writePermissionId = _createdPermissions.FirstOrDefault(p_ => p_.Name == Permission.Write.ToString())?.Id;
            var readPermissionId = _createdPermissions.FirstOrDefault(p_ => p_.Name == Permission.Read.ToString())?.Id;


            // 1. Admin role: admin (globally)
            var saveResult = SaveRolePermission(adminRoleId, adminPermissionId);
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return (ObjectResult) saveResult;

            // 2. Admin role: admin (Chinook)
            saveResult = SaveRolePermission(adminRoleId, adminPermissionId, "Chinook");
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return (ObjectResult) saveResult;

            // 3. User role: write (globally)
            saveResult = SaveRolePermission(userRoleId, writePermissionId);
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return (ObjectResult) saveResult;

            // 4. Anonymous role: read (globally)
            saveResult = SaveRolePermission(anonymousRoleId, readPermissionId);
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return (ObjectResult) saveResult;

            return Ok("Save roles permissions OK.");
        }

        private IActionResult SaveUserPermission()
        {
            var adminPermissionId = _createdPermissions.FirstOrDefault(p_ => p_.Name == Permission.Admin.ToString())?.Id;

            // John (admin user): Admin (globally)
            var saveResult = SaveUserPermission(adminPermissionId, _createdUsers[0]);
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
            {
                return saveResult;
            }

            // Paul : Admin (Chinook)
            // Note: Chinook is not distributed
            saveResult = SaveUserPermission(adminPermissionId, _createdUsers[2], "Chinook");
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
            {
                return saveResult;
            }

            return Ok("Save Users Permissions OK.");
        }

        /// <summary>
        /// Save users and fill _createdUsers class variable;
        /// </summary>
        /// <returns></returns>
        private async Task<ObjectResult> SaveUsers()
        {
            // our default user
            User johnUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@softinux.com",
                UserName = "johndoe",
                LockoutEnabled = false
            };

            // second user
            User janeUser = new User
            {
                FirstName = "Jane",
                LastName = "Fonda",
                Email = "janefonda@softinux.com",
                UserName = "janefonda",
                LockoutEnabled = false
            };

            // third user
            User paulUser = new User
            {
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
                        return StatusCode(500, ("\"(CreateAsync) Error creating user: { @userEmail }\"", user.Email));
                    }

                    // Assign roles to user. John has Admin role, Jane and Paul have User role
                    result = await _userManager.AddToRolesAsync(user, new[] {firstUser ? "Administrator" : "User"});

                    if (!result.Succeeded) // return 500 if fails
                    {
                        _logger.LogCritical("\"(AddToRolesAsync) Error adding user to role, user: { @userEmail }, role: Administrator\"", user.Email);
                        StatusCode(500, ("\"(AddToRolesAsync) Error adding user to role, user: { @userEmail }, role: Administrator\"", user.Email));
                    }
                }

                firstUser = false;
            }
            _createdUsers = new[] { johnUser, janeUser, paulUser};
            return Ok("Creating users OK.");
        }

        /// <summary>
        /// Save the roles and populate _createdRoles class variable.
        /// </summary>
        /// <returns></returns>
        private async Task<ObjectResult> SaveRoles()
        {
            // Get the list of the role from the enum
            Role[] roles = (Role[]) Enum.GetValues(typeof(Role));

            foreach (var r in roles)
            {
                // create an identity role object out of the enum value
                IdentityRole<string> identityRole = new IdentityRole<string>
                {
                    // Automatic ID
                    Name = r.GetRoleName()
                };
                _createdRoles.Add(identityRole);

                if (!await _roleManager.RoleExistsAsync(identityRole.Name))
                {
                    var result = await _roleManager.CreateAsync(identityRole);

                    //return 500 if fail
                    if (!result.Succeeded)
                    {
                        _logger.LogCritical("\"(CreateAsync) Error creating role: { @Name }\"", identityRole.Name);
                        return StatusCode(500, ("\"(CreateAsync) Error creating role: { @Name }\"", identityRole.Name));
                    }
                }
            }

            return Ok("Creating roles OK.");
        }

        /// <summary>
        /// Save the roles and populate _createdPermissions class variable.
        /// </summary>
        /// <returns></returns>
        private IActionResult SavePermissions()
        {
            Permission[] permissions = (Permission[])Enum.GetValues(typeof(Permission));

            foreach(var p in permissions)
            {
                // create a permission object out of the enum value
                Security.Data.Entities.Permission permission = new Security.Data.Entities.Permission()
                {
                    // Id is generated by database
                    Name = p.GetPermissionName()
                };

                _storage.GetRepository<IPermissionRepository>().Create(permission);
                _createdPermissions.Add(permission);
            }

             try
            {
                _storage.Save();

                _logger.LogInformation("\"Saving permissions ok.\"");
                return Ok($"Saving permissions ok.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("\"Error saving permission (@permission): {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", "", e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot save permissions. Error: {e.Message}");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="permissionId_"></param>
        /// <param name="user_"></param>
        /// <param name="scope_"></param>
        /// <returns></returns>
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
                    userPermission.Extension = scope_;

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="roleId_"></param>
        /// <param name="permissionId_"></param>
        /// <param name="extension_"></param>
        /// <returns></returns>
        private IActionResult SaveRolePermission(string roleId_, string permissionId_, string extension_ = null)
        {
            if (
                (!string.IsNullOrWhiteSpace(permissionId_)) &&
                (!string.IsNullOrWhiteSpace(roleId_))
                )
            {
                RolePermission rolePermission = new RolePermission()
                {
                    RoleId = roleId_,
                    PermissionId = permissionId_,
                };
                if(!string.IsNullOrWhiteSpace(extension_))
                    rolePermission.Extension = extension_;

                _storage.GetRepository<IRolePermissionRepository>().Create(rolePermission);
            }

            try
            {
                _storage.Save();
                _logger.LogInformation($"\"Saving role-permission: permission: {permissionId_}, to role: {roleId_}, for extension: {extension_ ?? "SoftinuxBase.Security"} ok.\"");
                return Ok("Saving role-permission ok.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("\"Error saving role-permission (@rolePermission, @permissionId): {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", roleId_, permissionId_, e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot save role-permission. Error: {e.Message}");
            }
        }

    }
}
