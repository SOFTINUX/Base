// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Security.ViewModels.Account;
using Security.ViewModels.Permissions;
using Security.Data.EntityFramework;

namespace Security.Tools
{
    public static class RegisterUser
    {
        /// <summary>
        /// Check if user exists by this name or e-mail.
        /// </summary>
        /// <returns>true if exist, false if not exist</returns>
        public static bool IsUserExist(IStorage storage_, string value_)
        {
            // TODO @xarkam normalize value_ (use UserManager.NormalizeKey()) and compare it, in the repository, to NormalizedName and NormalizedEmail.
            return storage_.GetRepository<IAspNetUsersRepository>().FindByUserNameOrEmail(value_);
        }

        /// <summary>
        /// Create user into application
        /// </summary>
        /// <param name="viewModel_">user object from view</param>
        /// <returns>true if create, false if fail</returns>
        public async static Task<bool> CreateNewUser(IStorage storage_,SignUpViewModel viewModel_)
        {
            if (IsUserExist(storage_, viewModel_.UserName))
            {
                return false;
            }
            if (IsUserExist(storage_, viewModel_.Email))
            {
                return false;
            }
            return true;
        }
    }
}