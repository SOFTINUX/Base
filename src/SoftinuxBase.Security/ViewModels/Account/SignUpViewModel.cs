// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SoftinuxBase.Security.ViewModels.Account
{
    public class SignUpViewModel
    {
        /// <summary>
        /// Gets or sets first name field.
        /// </summary>
        [Required]
        [Display(Name = "User Name")]
        [StringLength(256, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 4)]
        [Remote("CheckUserNameExist", "Account", HttpMethod = "POST", ErrorMessage = "User name already taken.")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets e-mail field.
        /// </summary>
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[0-9a-z-A-Z]+([0-9a-z-A-Z]*[-._+])*[0-9a-z-A-Z]+@[0-9a-z-A-Z]+([-.][0-9a-z-A-Z]+)*([0-9a-z-A-Z]*[.])[a-zA-Z]{2,6}$", ErrorMessage = "Incorrect Email format!")]
        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password field.
        /// </summary>
        [Display(Name = "Password")]
        [DataType(DataType.Password)]

        // [CustomValidation(typeof(Security.Permissions.Attributes.PasswordAttributes),"PasswordStrongTest")]
        [Required]
        [StringLength(10, MinimumLength = 8)]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
    }
}