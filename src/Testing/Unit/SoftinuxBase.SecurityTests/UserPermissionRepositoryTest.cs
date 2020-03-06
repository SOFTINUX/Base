// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using SoftinuxBase.Security.Permissions;
using SoftinuxBase.Security.Permissions.Enums;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Tests.Common;
using Xunit;
using Constants = SoftinuxBase.Security.Permissions.Constants;

namespace SoftinuxBase.SecurityTests
{
    [Collection("Database collection")]
    public class UserPermissionRepositoryTest : CommonTestWithDatabase
    {
        public UserPermissionRepositoryTest(DatabaseFixture databaseFixture_) : base(databaseFixture_)
        {
        }

        /// <summary>
        /// Permission level "Admin" for extension "SoftinuxBase.Security" is directly linked to an user: John Doe.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task FindByExtensionAndLevel_ShouldFindAsync()
        {
            var record = DatabaseFixture.Storage.GetRepository<IUserPermissionRepository>()
                .FindBy(Constants.SoftinuxBaseSecurity, Permission.Admin);
            Assert.NotEmpty(record);

            // It should be John Doe
            string johnDoeUserId = (await DatabaseFixture.UserManager.FindByNameAsync("johndoe"))?.Id;
            Assert.NotNull(johnDoeUserId);
            Assert.Equal(johnDoeUserId, record.First().UserId);
        }

        /// <summary>
        /// Permission level "Admin" for extension "SoftinuxBase.DoesNotExist" is not linked to any user.
        /// </summary>
        [Fact]
        public void FindByExtensionAndLevel_ShouldNotFind()
        {
            var record = DatabaseFixture.Storage.GetRepository<IUserPermissionRepository>()
                .FindBy("SoftinuxBase.DoesNotExist", Permission.Admin);
            Assert.Empty(record);
        }
    }
}
