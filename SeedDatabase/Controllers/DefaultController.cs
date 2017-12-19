// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SeedDatabase.Controllers
{
    [Route("dev/seed")]
    public class SeedDatabaseController : Controller
    {
        private readonly UserManager<Security.Data.Entities.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedDatabaseController(UserManager<Security.Data.Entities.User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
                if (r.GetRoleName() == "administrator"){
                    // create an idenity role object out of the enum value
                    IdentityRole identityRole = new IdentityRole {
                        Id = roles[0].GetRoleName(),
                        Name = roles[0].GetRoleName()
                    };

                     // our default user
                    Security.Data.Entities.User user = new Security.Data.Entities.User {
                        FirstName = "Doe",
                        LastName = "John",
                        Email = "johndoe@example.com",
                        UserName = "johndoe@example.com",
                        LockoutEnabled = false
                    };
                }
            }


            return Ok();
        }
    }
}