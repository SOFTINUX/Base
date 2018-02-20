// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ExtCore.Data.Abstractions;
using Infrastructure.Attributes;
using Microsoft.Extensions.Logging;
using Security.Data.Entities;
using ControllerBase = Infrastructure.ControllerBase;

namespace Security.Controllers
{
    [PermissionRequirement("Admin", "Security")]
    public class ListUsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserManager<User> _usersmanager;
        //public  List<IdentityUserRole<string>> Roles { get; set; }

        public ListUsersController(IStorage storage_, UserManager<User> users_, ILoggerFactory loggerFactory_) : base(storage_, loggerFactory_)
        {
            _logger = _loggerFactory.CreateLogger(GetType().FullName);
            _logger.LogInformation("oups");

            _usersmanager = users_;
        }

        // GET
        [Route("administration/listusers")]
        public IActionResult Index()
        {
            ViewBag.userList = _usersmanager.Users.Select(u => new SelectListItem { Text = u.UserName, Value = u.Id }).ToList();
            return View("ListUsers");
        }

        // GET
        [Route("administration/listusers/edituser")]
        public IActionResult EditUser(string userId_)
        {
            var user = _usersmanager.Users.FirstOrDefault(u_ => u_.Id == userId_);
            return View("Admin_Edit_User", user);
        }
    }
}