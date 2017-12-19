// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SeedDatabase.Controllers
{
    [Route("dev/seed")]
    public class SeedDatabaseController : Controller
    {
        private readonly UserManager<Security.Data.Entities.User> _userManager;
        private readonly RoleManager<Security.Data.Entities.Role> _roleManager;

        public SeedDatabaseController(UserManager<Security.Data.Entities.User> userManager, RoleManager<Security.Data.Entities.Role> roleManager)
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
                    // create an idenity role object out of the enum value
                    Security.Data.Entities.Role identityRole = new Security.Data.Entities.Role {
                        // ID is auto-generated
                        Name = r.GetRoleName(),
                        NormalizedName = r.GetRoleName().ToUpperInvariant()
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
                Email = "johndoe@example.com",
                UserName = "johndoe@example.com",
                LockoutEnabled = false
            };

            // add the user to the database if it doesn't already exist
            if (await _userManager.FindByEmailAsync(user.Email) == null)
            {
                // WARNING:: Do Not check in credentials of any kind into source control
                var result = await _userManager.CreateAsync(user, password: "5ESTdYB5cyYwA2dKhJqyjPYnKUc&45Ydw^gz^jy&FCV3gxpmDPdaDmxpMkhpp&9TRadU%wQ2TUge!TsYXsh77Qmauan3PEG8!6EP");

                if (!result.Succeeded) //return 500 if it fails
                    return StatusCode(StatusCodes.Status500InternalServerError);

                //Assign all roles to the default user
                result = await _userManager.AddToRolesAsync(user, roles.Select(r => r.GetRoleName()));

                if (!result.Succeeded) // return 500 if fails
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }
    }
}
