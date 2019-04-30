using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CommonTest;
using SoftinuxBase.Security.Common.Enums;
using SoftinuxBase.Security.Data.Abstractions;
using Xunit;

namespace SecurityTest
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
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous unit test.</placeholder></returns>
        [Fact]
        public async Task FindByExtensionAndLevel_ShouldFind()
        {
            var record = DatabaseFixture.Storage.GetRepository<IUserPermissionRepository>()
                .FindBy("SoftinuxBase.Security", Permission.Admin);
            Assert.NotNull(record);

            // It should be John Doe
            string johnDoeUserId = (await DatabaseFixture.UserManager.FindByNameAsync("johndoe"))?.Id;
            Assert.NotNull(johnDoeUserId);
            Assert.Equal(johnDoeUserId, record.UserId);
        }

        /// <summary>
        /// Permission level "Admin" for extension "SoftinuxBase.DoesNotExist" is not linked to any user.
        /// </summary>
        [Fact]
        public void FindByExtensionAndLevel_ShouldNotFind()
        {
            var record = DatabaseFixture.Storage.GetRepository<IUserPermissionRepository>()
                .FindBy("SoftinuxBase.DoesNotExist", Permission.Admin);
            Assert.Null(record);
        }
    }
}
