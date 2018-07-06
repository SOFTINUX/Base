// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace Security.ViewModels.Account
{
    public class SignUpViewModel
    {
        /// <summary>
        /// First name field
        /// </summary>
        /// <returns></returns>
        [Required]
        [StringLength(64)]
        [Display(Name = "First Name")]
        public string UserName { get; set; }

        /// <summary>
        /// E-mail field
        /// </summary>
        [Display(Name = "Email")]
        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// Password field
        /// </summary>
        [Display(Name = "Password")]
        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
    }
}