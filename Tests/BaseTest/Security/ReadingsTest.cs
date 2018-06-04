using System;
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
        public ReadingsTest(DatabaseFixture fixture_, ITestOutputHelper outputHandler_)
        {
            _fixture = fixture_;
            _outputHandler = outputHandler_;
        }

        /// <summary>
        /// Test that we can read and create roles into database, if it was empty.
        /// </summary>
        [Fact]
        public void TestReadCreateRoles()
        {
            // TODO find how to get access to initialized UserManager
            // Assert.True(_roleManager.Roles.Count() >= 0, "Expected 0 or more roles");
            // DataSetupTools.CreateBaseRolesIfNeeded(_roleManager);
            // Assert.True(_roleManager.Roles.Count() > 0, "Expected some roles");
            throw new NotImplementedException("Needs work");
        }

        /// <summary>
        /// Test that we can read and create permissions into database, if it was empty.
        /// </summary>
        [Fact]
        public void TestReadCreatePermissions()
        {
            var repository = Storage.GetRepository<IPermissionRepository>();
            Assert.True(repository.All().Count() >= 0, "Expected 0 or more permissions");
            DataSetupTools.CreatePermissionsIfNeeded(Storage);
            Assert.True(repository.All().Count() > 0, "Expected some permissions");
        }
    }
}