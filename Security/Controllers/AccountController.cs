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
            return this.View();
        }

        /// <summary>
        /// Sign in action request.
        /// </summary>
        /// <param name="signIn_"></param>
        /// <returns></returns>
        [HttpPost]
        //[ExportModelStateToTempData]
        [AllowAnonymous]
        public IActionResult SignIn(SignInViewModel signIn_)
        {
            // Check required fields
            if (!this.ModelState.IsValid)
                return this.CreateRedirectToSelfResult();

            UserManager userManager = new UserManager(this, LoggerFactory);
            User user = userManager.Login(Enums.CredentialType.Email, signIn_.Email, signIn_.Password);

            // Login failure: return to login page
            if (user == null)
            {
                signIn_.ErrorMessage = "Sign in failed";
                return this.View(signIn_);

                // return this.CreateRedirectToSelfResult();
            }
            userManager.LoadClaims(user, signIn_.RememberMe);
            
            // Go to dashboard, action Index of Barebone's controller
            return this.RedirectToAction("Index", "Barebone");
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            new UserManager(this, LoggerFactory).SignOut();
            return this.RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return this.View();
        }
    }
}
