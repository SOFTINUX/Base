using System;
using System.Linq;
using CommonTest;
using Security.Data.Abstractions;
using Security.Tools;
using Security.ViewModels.Permissions;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class BasicReadingTest : CommonTestWithDatabase
    {
        public BasicReadingTest(DatabaseFixture databaseFixture_) : base(databaseFixture_)
        {

        }

        /// <summary>
        /// Test that we are able to read data using Storage and ExtCore repository pattern.
        /// </summary>
        [Fact]
        public void TestReadWithRepository()
        {
            var data = DatabaseFixture.Storage.GetRepository<IPermissionRepository>().All();
            Assert.NotNull(data);
        }

        /// <summary>
        /// Test that we are able to read data using Identity.
        /// </summary>
        [Fact]
        public void TestReadWithIdentity()
        {
            var data = DatabaseFixture.RoleManager.Roles.ToList();
            Assert.NotNull(data);
        }
    }
}