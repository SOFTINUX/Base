// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security.Data.Entities;
using Security.ViewModels.Account;
using ControllerBase = Infrastructure.ControllerBase;

namespace Security.Controllers
{
    public class AccountController : ControllerBase
    {
        public AccountController(IStorage storage_, ILoggerFactory loggerFactory_) : base(storage_, loggerFactory_)
        {

        }

        /// <summary>
        /// Access to the login page with GET method, and allowed when not authenticated (of course ^^).
        /// </summary>
        /// <returns>SignIn view</returns>
        [HttpGet]
        //[ImportModelStateFromTempData]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        /// <summary>
        /// Sign in action request.
        /// </summary>
        /// <param name="signIn_"></param>
        /// <returns></returns>
        [HttpPost]
        //[ExportModelStateToTempData]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult SignIn(SignInViewModel signIn_)
        {
            // Check required fields, if any empty return to login page
            if (!ModelState.IsValid)
            {
                signIn_.ErrorMessage = "Unexpected error.";
                ModelState.AddModelError("UnexpectedError", signIn_.ErrorMessage);
                return View(signIn_);
            }

            UserManager userManager = new UserManager(this, LoggerFactory);
            // For now we support only "email" credential type provided by Security extension. Later we will support other types (LDAP...)
            User user = userManager.Login(Enums.CredentialType.Email, signIn_.Email, signIn_.Password, "Security");

            // Login failure: return to login page
            if (user == null)
            {
                signIn_.ErrorMessage = "Incorrect user name or password.";
                ModelState.AddModelError("BadUserPassword", signIn_.ErrorMessage);
                return View(signIn_);
            }
            userManager.LoadClaims(user, signIn_.RememberMe);

            // Go to dashboard, action Index of Barebone's controller
            return RedirectToAction("Index", "Barebone");
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            new UserManager(this, LoggerFactory).SignOut();
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateOrEditUser(uint userId_)
        {
            //TODO: charger le user ou préparer création nouveau user.
            return null;
        }

        [HttpPost]
        public IActionResult SaveUser(uint userId_)
        {
            return null;
        }


    }
}
