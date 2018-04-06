// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace Security.ViewModels.Account
{
    public class SignInViewModel
    {
        /// <summary>
        /// The user id, if any match
        /// </summary>
        public int? Id { get; set; }

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

        /// <summary>
        /// Use persistent cookie if desired
        /// </summary>
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string ErrorMessage { get; set; }
    }
}
