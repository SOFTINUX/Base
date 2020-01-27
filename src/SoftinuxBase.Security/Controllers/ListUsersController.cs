// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.FeatureAuthorize.PolicyCode;

namespace SoftinuxBase.Security.Controllers
{
    public class ListUsersController : Infrastructure.ControllerBase
    {
        // private readonly ILogger _logger;
        private readonly UserManager<User> _usersmanager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListUsersController"/> class.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="users_">.Net Identity user from UserManager.</param>
        /// <param name="loggerFactory_">application logger instance.</param>
        public ListUsersController(IStorage storage_, UserManager<User> users_, ILoggerFactory loggerFactory_) : base(storage_, loggerFactory_)
        {
            // _logger = LoggerFactory.CreateLogger(GetType().FullName);
            _usersmanager = users_;
        }

        /// <summary>
        /// List users.
        /// </summary>
        /// <returns>return listing view of users.</returns>
        [Route("administration/list-users")]
        [HttpGet]
        [ActionName("Index")]
        [HasPermission(typeof(Common.Enums.Permissions), (short)Common.Enums.Permissions.ListUsers)]
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.userList = _usersmanager.Users.Select(u_ => new SelectListItem { Text = u_.UserName, Value = u_.Id }).ToList();
            return await Task.Run(() => View("ListUsers"));
        }

        /// <summary>
        /// edit user.
        /// </summary>
        /// <param name="userId_">string represent user Id.</param>
        /// <returns>return edit user view.</returns>
        [Route("administration/list-users/edit-user")]
        [HttpGet]
        [ActionName("EditUser")]
        [HasPermission(typeof(Common.Enums.Permissions), (short)Common.Enums.Permissions.EditUsers)]
        public async Task<IActionResult> EditUserAsync(string userId_)
        {
            var user = _usersmanager.Users.FirstOrDefault(u_ => u_.Id == userId_);
            return await Task.Run(() => View("Admin_Edit_User", user));
        }
    }
}