// // Copyright © 2017 SOFTINUX. All rights reserved.
// // Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

// using BaseTest.Common;
// using BaseTest.Common.XunitTools;
// using ExtCore.Data.Abstractions;
// using Microsoft.AspNetCore.Identity;
// using Xunit;
// using Xunit.Abstractions;

// namespace BaseTest.Security
// {
//     [Collection("Database collection")]
//     public class GrantPermissionsTest : Common.BaseTest
//     {
//         public GrantPermissionsTest(DatabaseFixture fixture_, ITestOutputHelper outputHandler_, IStorage storage_, UserManager<Security.Data.Entities.User> userManager_, RoleManager<IdentityRole<string>> roleManager_)
//         {
//             _fixture = fixture_;
//             _outputHandler = outputHandler_;
//             _storage = storage_;
//             _userManager = userManager_;
//             _roleManager = roleManager_;

//         }

// /// <summary>
// /// Setup of custom data for role-permission, role-user links.
// /// Expect view model to be filled with some data according to database data.
// /// </summary>
//         [Fact, TestPriority(1)]
//         public void TestFillGrantPermissionViewModelForRoles()
//         {
//             // 1. Delete some data: role-permission, role-user, user-permission links.
//             DataSetupTools.DeleteAllLinks(_storage, _userManager, _roleManager);

//             // 2. Create "SpecialUser" additional role
//             // TODO

//             // 3. Setup links data
//             // TODO

//             // 4. Build GrantPermissionsViewModel and perform assertions
//             // TODO



//         }
//     }
// }