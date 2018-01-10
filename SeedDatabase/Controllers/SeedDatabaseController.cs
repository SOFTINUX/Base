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
        public IActionResult Get()
        {
            return Ok("Hello world!");
        }

        [HttpPost]
        public async Task<IActionResult> Index()
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

            return Ok("User Creation Ok.");
        }

        [HttpPost("/dev/seed/permissions")]
        public IActionResult Permissions()
        {
            IPermissionRepository repo = _storage.GetRepository<IPermissionRepository>();
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

                repo.Create(permission);
            }
            _storage.Save();

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
