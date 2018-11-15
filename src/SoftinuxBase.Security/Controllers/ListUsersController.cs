// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Security.Common.Attributes;
using SoftinuxBase.Security.Data.Entities;
using ControllerBase = SoftinuxBase.Infrastructure.ControllerBase;
using Permission = SoftinuxBase.Security.Common.Enums.Permission;

namespace SoftinuxBase.Security.Controllers
{
    [PermissionRequirement(Permission.Admin)]
    public class ListUsersController : Infrastructure.ControllerBase
    {
        //private readonly ILogger _logger;
        private readonly UserManager<User> _usersmanager;
        //public  List<IdentityUserRole<string>> Roles { get; set; }

        public ListUsersController(IStorage storage_, UserManager<User> users_, ILoggerFactory loggerFactory_) : base(storage_, loggerFactory_)
        {
            //_logger = _loggerFactory.CreateLogger(GetType().FullName);
            _usersmanager = users_;
        }

        [Route("administration/listusers")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.userList = _usersmanager.Users.Select(u => new SelectListItem { Text = u.UserName, Value = u.Id }).ToList();
            return await Task.Run(() => View("ListUsers"));
        }

        [Route("administration/listusers/edituser")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string userId_)
        {
            var user = _usersmanager.Users.FirstOrDefault(u_ => u_.Id == userId_);
            return await Task.Run(() => View("Admin_Edit_User", user));
        }
    }
}