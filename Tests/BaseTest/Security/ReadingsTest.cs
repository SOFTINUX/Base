using System.Linq;
using BaseTest.Common;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Security.Data.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace BaseTest.Security
{
    [Collection("Database collection")]
    public class ReadingsTest : Common.BaseTest
    {
        public ReadingsTest(DatabaseFixture fixture_, ITestOutputHelper outputHandler_, IStorage storage_, UserManager<global::Security.Data.Entities.User> userManager_, RoleManager<IdentityRole<string>> roleManager_)
        {
            _fixture = fixture_;
            _outputHandler = outputHandler_;
            _storage = storage_;
            _userManager = userManager_;
            _roleManager = roleManager_;

        }

        /// <summary>
        /// Test that we can read and create roles into database, if it was empty.
        /// </summary>
        [Fact]
        public void TestReadCreateRoles()
        {
            Assert.True(_roleManager.Roles.Count() >= 0, "Expected 0 or more roles");
            DataSetupTools.CreateBaseRolesIfNeeded(_roleManager);
            Assert.True(_roleManager.Roles.Count() > 0, "Expected some roles");
        }

        /// <summary>
        /// Test that we can read and create permissions into database, if it was empty.
        /// </summary>
        [Fact]
        public void TestReadCreatePermissions()
        {
            var repository = _storage.GetRepository<IPermissionRepository>();
            Assert.True(repository.All().Count() >= 0, "Expected 0 or more permissions");
            DataSetupTools.CreatePermissionsIfNeeded(_storage);
            Assert.True(repository.All().Count() > 0, "Expected some permissions");
        }
    }
}