// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.Data.Entities;
using SoftinuxBase.Security.ViewModels.Account;

namespace SoftinuxBase.Security.Tools
{
    /*
        The main RegsiterUser class.
        Contains all methods for registering new users.
    */
    /// <summary>
    /// The main RegsiterUser class.
    ///
    /// Contains all methods for registering new users.
    /// </summary>
    internal static class RegisterUser
    {
        /// <summary>
        /// Check if user exists by this name or e-mail.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="value_">User name or email.</param>
        /// <param name="userManager_">Users manager instance.</param>
        /// <returns>True if user exists, otherwise false.</returns>
        internal static bool IsUserExist(IStorage storage_, string value_, UserManager<User> userManager_)
        {
            return storage_.GetRepository<IAspNetUsersRepository>().FindByNormalizedUserNameOrEmail(userManager_.NormalizeKey(value_));
        }

        /// <summary>
        /// Create user into application.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="viewModel_">user object from view.</param>
        /// <param name="userManager_">the users manager instance.</param>
        /// <returns>True if user created, return false if fail.</returns>
        internal static bool CreateNewUser(IStorage storage_, SignUpViewModel viewModel_, UserManager<User> userManager_)
        {
            return !IsUserExist(storage_, viewModel_.UserName, userManager_) && !IsUserExist(storage_, viewModel_.Email, userManager_);
        }
    }
}