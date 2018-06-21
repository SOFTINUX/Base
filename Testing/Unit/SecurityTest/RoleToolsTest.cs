using System;
using CommonTest;
using Security.Tools;
using Security.ViewModels.Permissions;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class RoleToolsTest : TestWithDatabase
    {
        public RoleToolsTest(DatabaseFixture databaseFixture_) : base(databaseFixture_)
        {

        }

        [Fact]
        public void TestCheckAndSaveNewRole_Ok()
        {
            // Arrange
            // SaveNewRoleViewModel model_, RoleManager<IdentityRole<string>> roleManager_, IStorage storage_
            SaveNewRoleViewModel model = new SaveNewRoleViewModel
            {
                // Really unique value
                Role = "New Role 1 " + DateTime.Now.Ticks,
                Extensions = new System.Collections.Generic.List<string> { "Security" },
                Permission = Security.Common.Enums.Permission.Write.ToString()
            };
            // TODO get RoleManager into test system and use it below instead of "null"

            // Execute
            var result = RoleTools.CheckAndSaveNewRole(model, null, DatabaseFixture.Storage).Result;
            Assert.Equal(string.Empty, result);
        }
    }
}