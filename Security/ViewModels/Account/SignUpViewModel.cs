// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Security.ViewModels.Account
{
    public class SignUpViewModel
    {
        /// <summary>
        /// First name field
        /// </summary>
        /// <returns></returns>
        [Required]
        [Display(Name = "User Name")]
        [StringLength(12, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 4)]
        [Remote("CheckUserNameExist", "Account", HttpMethod = "POST", ErrorMessage = "User name already taken.")]
        public string UserName { get; set; }

        /// <summary>
        /// E-mail field
        /// </summary>
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[0-9a-z-A-Z]+([0-9a-z-A-Z]*[-._+])*[0-9a-z-A-Z]+@[0-9a-z-A-Z]+([-.][0-9a-z-A-Z]+)*([0-9a-z-A-Z]*[.])[a-zA-Z]{2,6}$", ErrorMessage = "Incorrect Email format!")]
        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// Password field
        /// </summary>
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [CustomValidation(typeof(Security.Common.Attributes.PasswordAttributes),"PasswordStrongTest")]
        [Required]
        [StringLength(10, MinimumLength = 6)]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
    }
}