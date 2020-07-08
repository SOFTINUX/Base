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
using SoftinuxBase.Infrastructure.Interfaces;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Security.Permissions.Enums;

namespace SoftinuxBase.SeedDatabase.Controllers
{
    [Route("dev/seed")]
    public class SeedDatabaseController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IAspNetRolesManager _rolesManager;
        private readonly IStorage _storage;
        private readonly ILogger _logger;

        private readonly List<IdentityRole<string>> _createdRoles = new List<IdentityRole<string>>();

        // 0: John, 1: Jane, 2: Paul
        private User[] _createdUsers = new User[3];

        public SeedDatabaseController(UserManager<User> userManager_, ILoggerFactory loggerFactory_, IStorage storage_, IAspNetRolesManager rolesManager_)
        {
            _userManager = userManager_;
            _storage = storage_;
            _rolesManager = rolesManager_;
            _logger = loggerFactory_?.CreateLogger(GetType().FullName);
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Access REST API \"Hello world!\"");
            return Ok("Hello world!");
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

                // Save new PERMISSIONS
                await SavePermissionsAsync();

                return Ok("Demo database initialization Ok.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private async Task<IActionResult> SavePermissionsAsync()
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
            permissions.AddGrouped(typeof(Permissions), new List<short>
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
                    permissions));

            // Role: User, permissions: admin general access + list/read
            permissions = new PermissionsDictionary();
            permissions.AddGrouped(typeof(Permissions), new List<short>
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

                if (!await _rolesManager.RoleExistsAsync(identityRole.Name))
                {
                    var result = await _rolesManager.CreateAsync(identityRole);

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
    }
}
