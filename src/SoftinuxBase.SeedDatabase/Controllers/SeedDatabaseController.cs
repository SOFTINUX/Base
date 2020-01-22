// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Security.Common.Enums;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;

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

        public SeedDatabaseController(UserManager<User> userManager_, RoleManager<IdentityRole<string>> roleManager_, ILoggerFactory loggerFactory_, IStorage storage_)
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
        [ActionName("SetNewPermissions")]
        [Route("/dev/seed/create-users-permissions")]
        public async Task<IActionResult> CreateUsersPermissionsAsync()
        {
            try
            {
                // Save RoleToPermissions records
                await SaveRoleToPermissions();

                // Save UserToRoles records
                var saveResults = await SaveUserToRoles();

                // Verify data
                VerifySavedData(saveResults);

                return Ok("New permissions system initialization Ok.");
            }
            catch (Exception e)
            {
                return StatusCode(500, $"{e.Message} - {e.StackTrace}");
            }
        }

        private void VerifySavedData(IEnumerable<bool> userToRoleSaveResults_)
        {
            var roleToPermissionsRepo = _storage.GetRepository<IRoleToPermissionsRepository>();
            if (!roleToPermissionsRepo.All().Any())
            {
                throw new Exception("Role to permissions NOT SAVED");
            }

            foreach (var result in userToRoleSaveResults_)
            {
                if (!result)
                {
                    throw new Exception("User to roles NOT SAVED");
                }
            }
        }

        private async Task<IEnumerable<bool>> SaveUserToRoles()
        {
            var userToRoleRepo = _storage.GetRepository<IUserToRoleRepository>();

            // Cleanup
            userToRoleRepo.DeleteAll();

            List<bool> saveResults = new List<bool>();
            // John: Admin, Jane and Paul: User
            var johnDoeUser = await _userManager.FindByNameAsync("johndoe");
            var janeFondaUser = await _userManager.FindByNameAsync("janefonda");
            var paulKellerUser = await _userManager.FindByNameAsync("paulkeller");
            saveResults.Add(userToRoleRepo.AddUserToRole(johnDoeUser.Id, Role.Administrator.GetRoleName()));
            saveResults.Add(userToRoleRepo.AddUserToRole(janeFondaUser.Id, Role.User.GetRoleName()));
            saveResults.Add(userToRoleRepo.AddUserToRole(paulKellerUser.Id, Role.User.GetRoleName()));

            await _storage.SaveAsync();

            return saveResults;
        }

        private async Task SaveRoleToPermissions()
        {
            var roleToPermissionsRepo = _storage.GetRepository<IRoleToPermissionsRepository>();

            // Cleanup
            roleToPermissionsRepo.DeleteAll();

            // Role: Admin, permissions: all
            var permissions = new PermissionsDictionary();
           permissions.AddGrouped(typeof(Permissions).GetAssemblyShortName(), new List<short>
            {
                (short)Permissions.AccessAll,
                (short)Permissions.AccessExtension,
                (short)Permissions.Admin,
                (short)Permissions.Create,
                (short)Permissions.Delete,
                (short)Permissions.Edit,
                (short)Permissions.Read,
                (short)Permissions.CreateRoles,
                (short)Permissions.DeleteRoles,
                (short)Permissions.EditRoles,
                (short)Permissions.ListRoles,
                (short)Permissions.ReadRoles,
                (short)Permissions.CreateUsers,
                (short)Permissions.DeleteUsers,
                (short)Permissions.EditUsers,
                (short)Permissions.ListUsers,
                (short)Permissions.ReadUsers,
                (short)Permissions.EditUsersPermissions,
            });
            roleToPermissionsRepo.Create(
                new RoleToPermissions(
                    Role.Administrator.GetRoleName(),
                    "Administrator role",
                    permissions
                    ));

            // Role: User, permissions: admin general access + list/read
            permissions = new PermissionsDictionary();
            permissions.AddGrouped(typeof(Permissions).GetAssemblyShortName(), new List<short>
            {
                (short)Permissions.Admin,
                (short)Permissions.Read,
                (short)Permissions.ListRoles,
                (short)Permissions.ReadRoles,
                (short)Permissions.ListUsers,
                (short)Permissions.ReadUsers,
            });
            roleToPermissionsRepo.Create(
                new RoleToPermissions(
                    Role.User.GetRoleName(),
                    "User role",
                    permissions));

            await _storage.SaveAsync();
        }

        [HttpPost]
        [ActionName("CreateUser")]
        [Route("/dev/seed/create-user")]
        public async Task<IActionResult> CreateUserAsync()
        {
            try
            {
                // Save ROLES
                await SaveRolesAsync();

                // Save USERS and USER-ROLE
                await SaveUsersAsync();

                // Save PERMISSIONS
                await SavePermissionsAsync();

                // Save USER-PERMISSION
                await SaveUserPermissionAsync();

                // Save ROLE-PERMISSION
                await SaveRolePermissionAsync();

                return Ok("Demo database initialization Ok.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Save users and fill _createdUsers class variable;.
        /// </summary>
        private async Task SaveUsersAsync()
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
            IdentityResult result;
            string roleName;

            foreach (var user in new[] { johnUser, janeUser, paulUser })
            {
                // add the user to the database if it doesn't already exist
                if (await _userManager.FindByEmailAsync(user.Email) == null)
                {
                    // WARNING: Do Not check in credentials of any kind into source control
                    result = await _userManager.CreateAsync(user, password: "123_Password");

                    // return 500 if it fails
                    if (!result.Succeeded)
                    {
                        string msg = $"(SaveUsers: UserManager.CreateAsync) Error creating user: {user.Email}";
                        _logger.LogCritical(msg);
                        throw new Exception(msg);
                    }

                    // Assign roles to user. John has Admin role, Jane and Paul have User role
                    roleName = (firstUser ? Role.Administrator : Role.User).GetRoleName();
                    result = await _userManager.AddToRolesAsync(user, new[] { roleName });

                    // return 500 if fails
                    if (!result.Succeeded)
                    {
                        string msg = $"(SaveUsers: UserManager.AddToRolesAsync) Error adding user to role, user: {user.Email}, role: {roleName}";
                        _logger.LogCritical(msg);
                        throw new Exception(msg);
                    }
                }

                firstUser = false;
            }

            _createdUsers = new[] { johnUser, janeUser, paulUser };
        }

        /// <summary>
        /// Save the roles and populate _createdRoles class variable.
        /// </summary>
        private async Task SaveRolesAsync()
        {
            // Get the list of the role from the enum
            Role[] roles = (Role[])Enum.GetValues(typeof(Role));

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

                    // return 500 if fail
                    if (!result.Succeeded)
                    {
                        string msg = $"(SaveRoles: RoleManager.CreateAsync) Error creating role: {identityRole.Name}";
                        _logger.LogCritical(msg);
                        throw new Exception(msg);
                    }
                }
            }
        }

        /// <summary>
        /// TODO.
        /// </summary>
        private async Task SaveUserPermissionAsync()
        {
            var adminPermissionId = _createdPermissions.FirstOrDefault(p_ => p_.Name == Security.Common.Enums.Permission.Admin.ToString())?.Id;

            // John (admin user): Admin (globally)
            await SaveUserPermissionAsync(adminPermissionId, _createdUsers[0]);

            // Paul : Admin (Chinook)
            // Note: Chinook is not distributed
            await SaveUserPermissionAsync(adminPermissionId, _createdUsers[2], "Chinook");
        }

        /// <summary>
        /// TODO.
        /// </summary>
        private async Task SaveRolePermissionAsync()
        {
            var adminRoleId = _createdRoles.FirstOrDefault(r_ => r_.Name == Role.Administrator.ToString())?.Id;
            var userRoleId = _createdRoles.FirstOrDefault(r_ => r_.Name == Role.User.ToString())?.Id;
            var anonymousRoleId = _createdRoles.FirstOrDefault(r_ => r_.Name == Role.Anonymous.ToString())?.Id;

            var adminPermissionId = _createdPermissions.FirstOrDefault(p_ => p_.Name == Security.Common.Enums.Permission.Admin.ToString())?.Id;
            var writePermissionId = _createdPermissions.FirstOrDefault(p_ => p_.Name == Security.Common.Enums.Permission.Write.ToString())?.Id;
            var readPermissionId = _createdPermissions.FirstOrDefault(p_ => p_.Name == Security.Common.Enums.Permission.Read.ToString())?.Id;

            // 1. Admin role: admin (globally)
            await SaveRolePermissionAsync(adminRoleId, adminPermissionId);

            // 2. Admin role: admin (Chinook)
            await SaveRolePermissionAsync(adminRoleId, adminPermissionId, "Chinook");

            // 3. User role: write (globally)
            await SaveRolePermissionAsync(userRoleId, writePermissionId);

            // 4. Anonymous role: read (globally)
            await SaveRolePermissionAsync(anonymousRoleId, readPermissionId);
        }

        /// <summary>
        /// Save the roles and populate _createdPermissions class variable.
        /// </summary>
        private async Task SavePermissionsAsync()
        {
            Security.Common.Enums.Permission[] permissions = (Security.Common.Enums.Permission[])Enum.GetValues(typeof(Security.Common.Enums.Permission));

            foreach (var p in permissions)
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
                await _storage.SaveAsync();
                _logger.LogInformation("\"Saving permissions ok.\"");
            }
            catch (Exception e)
            {
                string msg = $"Cannot save permissions. Error: {e.Message} {e.StackTrace}";
                _logger.LogCritical(msg);
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="permissionId_">permission ID.</param>
        /// <param name="user_">Application user.</param>
        /// <param name="extension_">Extension name.</param>
        private async Task SaveUserPermissionAsync(string permissionId_, User user_, string extension_ = null)
        {
            if (!string.IsNullOrWhiteSpace(permissionId_) && user_ != null)
            {
                UserPermission userPermission = new UserPermission()
                {
                    UserId = user_.Id,
                    PermissionId = permissionId_
                };
                if (!string.IsNullOrWhiteSpace(extension_))
                {
                    userPermission.Extension = extension_;
                }

                _storage.GetRepository<IUserPermissionRepository>().Create(userPermission);
            }

            try
            {
                await _storage.SaveAsync();
                _logger.LogInformation($"\"Saving user-permission {permissionId_} ok.\"");
            }
            catch (Exception e)
            {
                string msg = $"Cannot save user-permission. User id: {user_.Id}, permission id: {permissionId_}, Error: {e.Message} {e.StackTrace}";
                _logger.LogCritical(msg);
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="roleId_">Role Id.</param>
        /// <param name="permissionId_">Permission ID.</param>
        /// <param name="extension_">Extension name.</param>
        private async Task SaveRolePermissionAsync(string roleId_, string permissionId_, string extension_ = null)
        {
            if ((!string.IsNullOrWhiteSpace(permissionId_)) && (!string.IsNullOrWhiteSpace(roleId_)))
            {
                RolePermission rolePermission = new RolePermission()
                {
                    RoleId = roleId_,
                    PermissionId = permissionId_,
                };
                if (!string.IsNullOrWhiteSpace(extension_))
                {
                    rolePermission.Extension = extension_;
                }

                _storage.GetRepository<IRolePermissionRepository>().Create(rolePermission);
            }

            try
            {
                await _storage.SaveAsync();
                _logger.LogInformation($"\"Saving role-permission: permission: {permissionId_}, to role: {roleId_}, for extension: {extension_ ?? Security.Common.Constants.SoftinuxBaseSecurity} ok.\"");
            }
            catch (Exception e)
            {
                string msg = $"Cannot save role-permission. role id: {roleId_}, permission id: {permissionId_}, extension: {extension_}, Error: {e.Message} {e.StackTrace}";
                _logger.LogCritical(msg);
                throw new Exception(msg);
            }
        }
    }
}
