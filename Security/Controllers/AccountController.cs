// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security.Data.Entities;
using Security.ViewModels.Account;
using Security.Tools;
using ControllerBase = Infrastructure.ControllerBase;

namespace Security.Controllers
{
    public class AccountController : ControllerBase
    {
        private SignInManager<User> _signInManager;
        private ILogger _logger;

        public AccountController(IStorage storage_, ILoggerFactory loggerFactory_, SignInManager<User> signInManager_) : base(storage_, loggerFactory_)
        {
            _signInManager = signInManager_;
            _logger = _loggerFactory.CreateLogger(GetType().FullName);
        }

        /// <summary>
        /// Access to the register page with GET method, and allowed when not authenticated (of course ^^).
        /// </summary>
        /// <returns>SignUp view</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        /// <summary>
        /// SignUp action request
        /// POST: /Account/SignUp
        /// </summary>
        /// <param name="signUp_"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(SignUpViewModel signUp_)
        {
            // Check required fields, if any empty return to signup page
            if (!ModelState.IsValid)
            {
                signUp_.ErrorMessage = "Form has error(s)";
                ModelState.AddModelError("IncompleteForm", signUp_.ErrorMessage);
                return View(signUp_);
            }

            // Create a new User object with data from signUp_
            //var user = new ApplicationUser { UserName = signUp_.UserName, Email = signUp_.Email };
            //var result = await UserManager.CreateAsync(user, model.Password);
            /*
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                return RedirectToAction("Index", "Home");
            }
            AddErrors(result);
            */

            return View("Index");
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
        public async Task<IActionResult> SignIn(SignInViewModel signIn_)
        {
            // Check required fields, if any empty return to login page
            if (!ModelState.IsValid)
            {
                signIn_.ErrorMessage = "Required data missing";
                ModelState.AddModelError("BadUserPassword", signIn_.ErrorMessage);
                return View(signIn_);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(signIn_.Email, signIn_.Password, signIn_.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in");

                // Go to dashboard, action Index of Barebone's controller
                return RedirectToAction("Index", "Barebone");
            }
            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
            //}
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out");
                //return RedirectToAction(nameof(Lockout));
                signIn_.ErrorMessage = "User account locked out";
                ModelState.AddModelError("BadUserPassword", signIn_.ErrorMessage);
            }
            else
            {
                //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                signIn_.ErrorMessage = "Invalid login attempt";
                ModelState.AddModelError("BadUserPassword", signIn_.ErrorMessage);
            }
            // If we got this far, something failed, redisplay form
            return View(signIn_);

        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateOrEditUser(string userId_)
        {
            return null;
        }

        [HttpPost]
        public IActionResult SaveUser(string userId_)
        {
            return null;
        }

        [HttpGet]
        public IActionResult EditProfile(string userId_)
        {
            return View("User");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CheckUserNameExist(string userName_)
        {
            // if (!RegisterUser.IsUserExist(Storage, userName_))
            // {
            //     _logger.LogWarning($"User {userName_} already taken");
            //     ModelState.AddModelError("UserName",$"User {userName_} already taken.");
            // }
            return Json(!RegisterUser.IsUserExist(Storage, userName_));
        }
    }
}
