// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using BaseTest.Common;
using BaseTest.Common.XunitTools;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Xunit;
using Xunit.Abstractions;

namespace BaseTest.Security
{
    [Collection("Database collection")]
    public class GrantPermissionsTest : Common.BaseTest
    {
        public GrantPermissionsTest(DatabaseFixture fixture_, ITestOutputHelper outputHandler_)
        {
            _fixture = fixture_;
            _outputHandler = outputHandler_;
        }

        /// <summary>
        /// Setup of custom data for role-permission, role-user links.
        /// Expect view model to be filled with some data according to database data.
        /// </summary>
        [Fact]
        public void TestFillGrantPermissionViewModelForRoles()
        {
            // TODO find how to get access to initialized UserManager
            // 1. Delete some data: role-permission, role-user, user-permission links.
            // DataSetupTools.DeleteAllLinks(Storage, _userManager, _roleManager);

            // 2. Create base roles and permissions, if needed
            // TODO

            // 3. Create "SpecialUser" additional role
            // TODO

            // 4. Setup links data
            // TODO

            // 5. Build GrantPermissionsViewModel and perform assertions
            // TODO

            throw new NotImplementedException("Needs work");

        }
    }
}