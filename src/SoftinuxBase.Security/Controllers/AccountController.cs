// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.Tools;
using SoftinuxBase.Security.ViewModels.Account;
using ControllerBase = SoftinuxBase.Infrastructure.ControllerBase;

namespace SoftinuxBase.Security.Controllers
{
    public class AccountController : Infrastructure.ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public AccountController(IStorage storage_, ILoggerFactory loggerFactory_, UserManager<User> userManager_, SignInManager<User> signInManager_) : base(storage_, loggerFactory_)
        {
            _userManager = userManager_;
            _signInManager = signInManager_;
            _logger = LoggerFactory.CreateLogger(GetType().FullName);
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
        public async Task<IActionResult> SignUp(SignUpViewModel signUp_)
        {
            // Check required fields, if any empty return to signup page
            if (!ModelState.IsValid)
            {
                signUp_.ErrorMessage = "Form has error(s)";
                ModelState.AddModelError("IncompleteForm", signUp_.ErrorMessage);
                return await Task.Run(() => View(signUp_));
            }

            return await Task.Run(() => View("Index"));
        }

        /// <summary>
        /// Access to the login page with GET method, and allowed when not authenticated (of course ^^).
        /// </summary>
        /// <returns>SignIn view</returns>
        [HttpGet]
        //[ImportModelStateFromTempData]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn()
        {
            return await Task.Run(() => View());
        }

        /// <summary>
        /// Sign in action request.
        /// </summary>
        /// <param name="signIn_"></param>
        /// <returns>View</returns>
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
                return await Task.Run(() => View(signIn_));
            }

            // find user by nickname or email
            if (signIn_.Username.Contains("@"))
            {
                var userForEmail = await _userManager.FindByEmailAsync(signIn_.Username);
                if (userForEmail != null)
                {
                    signIn_.Username = userForEmail.UserName;
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(signIn_.Username, signIn_.Password, signIn_.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in");

                // Go to dashboard, action Index of Barebone's controller
                return await Task.Run(() => RedirectToAction("Index", "Barebone"));
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
            return await Task.Run(() => View(signIn_));

        }

        /// <summary>
        /// SignOut action request
        /// </summary>
        /// <returns>View SignIn</returns>
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return await Task.Run(() => RedirectToAction("SignIn"));
        }

        /// <summary>
        /// No acces area
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return await Task.Run(() => View());
        }

        /// <summary>
        /// Create user action request
        /// </summary>
        /// <returns>view for creating new user</returns>
        [HttpGet]
        public IActionResult CreateUser()
        {
            // TODO coder: create this as async method
            // return user creation view
            return null;
        }

        [HttpPost]
        public IActionResult SaveUser(string userId_)
        {
            // TODO coder: create this as async method
            return null;
        }

        /// <summary>
        /// Update user profile action request
        /// </summary>
        /// <param name="userId_"></param>
        /// <returns>view user</returns>
        [HttpGet]
        public async Task<IActionResult> UpdateProfile(string userId_)
        {
            return await Task.Run(() => View("User"));
        }

        /// <summary>
        /// Return true when user name isn't in use.
        /// </summary>
        /// <param name="userName_"></param>
        /// <returns>json</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUserNameExist(string userName_)
        {
            return await Task.Run(() => Json(!RegisterUser.IsUserExist(Storage, userName_, _userManager)));
        }
    }
}
