// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Security;
using Security.Data.Abstractions;
using Microsoft.Extensions.Logging;

namespace SeedDatabase.Controllers
{
    [Route("dev/seed")]
    public class SeedDatabaseController : Controller
    {
        private readonly UserManager<Security.Data.Entities.User> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly IStorage _storage;
        protected ILogger _logger;

        public SeedDatabaseController(UserManager<Security.Data.Entities.User> userManager_,
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
                    Id = r.GetRoleName(),
                    Name = r.GetRoleName()
                };

                if(!await _roleManager.RoleExistsAsync(roleName: identityRole.Name))
                {
                    var result = await _roleManager.CreateAsync(identityRole);

                    //return 500 if fail
                    if(!result.Succeeded)
                    {
                        _logger.LogCritical("\"Error to get rolemanager async role: { @Name }\"", identityRole.Name);
                        return  StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }

            // our default user
            Security.Data.Entities.User admin = new Security.Data.Entities.User {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@softinux.com",
                UserName = "johndoe@softinux.com",
                LockoutEnabled = false
            };

            // second user
            Security.Data.Entities.User janeUser = new Security.Data.Entities.User {
                FirstName = "Jane",
                LastName = "Fonda",
                Email = "janefonda@softinux.com",
                UserName = "janefonda@softinux.com",
                LockoutEnabled = false
            };

            // second user
            Security.Data.Entities.User paulUser = new Security.Data.Entities.User {
                FirstName = "Paul",
                LastName = "Keller",
                Email = "paulkeller@softinux.com",
                UserName = "paulkeller@softinux.com",
                LockoutEnabled = false
            };

            bool firstUser = true;
            foreach (var user in new[] {admin, janeUser, paulUser})
            {
                // add the user to the database if it doesn't already exist
                        if (await _userManager.FindByEmailAsync(user.Email) == null)
                        {
                            // WARNING: Do Not check in credentials of any kind into source control
                            var result = await _userManager.CreateAsync(user, password: "123_Password");

                            if (!result.Succeeded) //return 500 if it fails
                            {
                                _logger.LogCritical("\"Error to CreateAsync user: { @userEmail }\"", user.Email);
                                return StatusCode(StatusCodes.Status500InternalServerError);
                            }

                            //Assign roles to user
                            result = await _userManager.AddToRolesAsync(user, new[] { firstUser ? "Administrator" : "User" });

                            if (!result.Succeeded) // return 500 if fails
                            {
                                _logger.LogCritical("\"Error to AddToRolesAsync user: { @userEmail }, role: Administrator\"", user.Email );
                                return StatusCode(StatusCodes.Status500InternalServerError);
                            }
                        }
                        firstUser = false;
            }
            // Save PERMISSIONS
            var saveResult = SavePermissions();
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // Save GROUPS
            saveResult = SaveGroup("ExtensionsAdministrators", "Extensions administrators");
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // Save USER-GROUP
            // Paul is in a group
            saveResult = SaveUserGroup("ExtensionsAdministrators", paulUser);
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // Save USER-PERMISSION

            // Admin
            saveResult = SaveUserPermission(Infrastructure.Enums.Permission.Admin.ToString(), admin);
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // Save ROLE-PERMISSION

            // 1. Admin role: admin, write, read
            saveResult = SaveRolePermission(Role.Administrator.ToString(), Infrastructure.Enums.Permission.Admin.ToString());
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            saveResult = SaveRolePermission(Role.Administrator.ToString(), Infrastructure.Enums.Permission.Write.ToString());
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            saveResult = SaveRolePermission(Role.Administrator.ToString(), Infrastructure.Enums.Permission.Read.ToString());
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // 2. User role: write, read
            saveResult = SaveRolePermission(Role.User.ToString(), Infrastructure.Enums.Permission.Write.ToString());
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            saveResult = SaveRolePermission(Role.User.ToString(), Infrastructure.Enums.Permission.Read.ToString());
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            saveResult = SaveRolePermission(Role.Administrator.ToString(), "Extension1.Admin");
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            // Save GROUP-PERMISSION
            // The group "ExtensionsAdministrators" (which contains Paul) has right for extension1 administration
            saveResult = SaveGroupPermission("Extension1.Admin", "ExtensionsAdministrators");
            if (saveResult.GetType() != typeof(OkObjectResult)) // return 500 if fails
                return saveResult;

            return Ok("User Creation Ok.");
        }

        private IActionResult SaveGroupPermission(string permissionId_, string groupId_)
        {
            if (!string.IsNullOrWhiteSpace(permissionId_) && !string.IsNullOrWhiteSpace(groupId_))
            {
                Security.Data.Entities.GroupPermission groupPermission = new Security.Data.Entities.GroupPermission()
                {
                    GroupId = groupId_,
                    PermissionId = permissionId_
                };

                _storage.GetRepository<IGroupPermissionRepository>().Create(groupPermission);
            }
            try
            {
                _storage.Save();

                _logger.LogInformation("\"Saving group-permission ok.\"");
                return Ok("Saving group-permission ok.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("\"Error to saving group-permission: {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot saving group-permission. Error: {e.Message}");
            }
        }

        private IActionResult SaveUserGroup(string groupId_, Security.Data.Entities.User user_)
        {
            if (!string.IsNullOrWhiteSpace(groupId_) && user_ != null)
            {
                Security.Data.Entities.UserGroup userGroup = new Security.Data.Entities.UserGroup()
                {
                    UserId = user_.Id,
                    GroupId = groupId_
                };

                _storage.GetRepository<IUserGroupRepository>().Create(userGroup);
            }
            try
            {
                _storage.Save();

                _logger.LogInformation("\"Saving user-group ok.\"");
                return Ok("Saving user-group ok.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("\"Error saving user-group: {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot saving user-group. Error: {e.Message}");
            }
        }

        private IActionResult SaveGroup(string id_, string name_)
        {
            Security.Data.Entities.Group extensionsAdminGroup = new Security.Data.Entities.Group
            {
                Id = id_,
                Name = name_
            };

            _storage.GetRepository<IGroupRepository>().Create(extensionsAdminGroup);

            try
            {
                _storage.Save();

                _logger.LogInformation("\"Saving group ok.\"");
                return Ok("Saving group ok.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("\"Error saving group: {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot saving group. Error: {e.Message}");
            }
        }

        private IActionResult SavePermissions()
        {
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

            // Permission for extension1 administration
            Security.Data.Entities.Permission permission1 = new Security.Data.Entities.Permission()
            {
                Id = "Extension1.Admin", // OriginExtension + Name
                Name = "Admin",
                OriginExtension = "Extension1"
            };

            _storage.GetRepository<IPermissionRepository>().Create(permission1);

            try
            {
                _storage.Save();

                _logger.LogInformation("\"Saving permissions ok.\"");
                return Ok("Saving permissions ok.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("\"Error saving permission: {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot saving permissions. Error: {e.Message}");
            }
        }

        private IActionResult SaveUserPermission(string permissionId_, Security.Data.Entities.User user_)
        {
            if (!string.IsNullOrWhiteSpace(permissionId_) && user_ != null)
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

                _logger.LogInformation("\"Saving user-permission ok.\"");
                return Ok("Saving user-permission ok.");
            }
            catch (Exception e)
            {
                _logger.LogCritical("\"Error saving user-permission: {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot saving user-permission. Error: {e.Message}");
            }
        }

        private IActionResult SaveRolePermission(string roleId_, string permissionId_)
        {
            if (
                (!string.IsNullOrWhiteSpace(permissionId_)) &&
                (!string.IsNullOrWhiteSpace(roleId_))
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
                _logger.LogInformation("\"Saving role-permission ok.\"");
                return Ok("Saving role-permission ok.");
            }
            catch (Exception e)
            {

                _logger.LogCritical("\"Error saving role-permission: {@message}, \n\rInnerException: {@innerException} \n\rStackTrace: {@stackTrace} \"", e.Message, e.InnerException, e.StackTrace );
                return StatusCode(StatusCodes.Status500InternalServerError, $"Cannot saving role-permission. Error: {e.Message}");
            }
        }

    }
}
