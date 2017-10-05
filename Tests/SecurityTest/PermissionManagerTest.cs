using Security;
using SecurityTest.Util;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Security.Enums;
using Xunit;
using Permission = Security.Data.Entities.Permission;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class PermissionManagerTest : BaseTest
    {
        private const string CST_PERM_CODE = "test_perm";
        private const string CST_TEST_PERM_RW_CLAIM_TYPE = "test_perm RW";
        private static string _assembly = typeof(PermissionManagerTest).Assembly.FullName;

        public PermissionManagerTest(DatabaseFixture fixture_)
        {
            _fixture = fixture_;
        }

        [Fact]
        public void TestGetFinalPermissionsRw()
        {
            Permission roPerm = new Permission { Code = CST_PERM_CODE, OriginExtension = _assembly };
            Permission rwPerm = new Permission { Code = CST_PERM_CODE, OriginExtension = _assembly };
            IEnumerable<Claim> claims = new PermissionManager().GetFinalPermissions(new List<Permission> { roPerm, rwPerm });
            Assert.Equal(1, claims.Count());
            Assert.Equal(ClaimType.Permission, claims.First().Type);
            Assert.Equal(CST_TEST_PERM_RW_CLAIM_TYPE, claims.First().Value);
        }

        [Fact]
        public void TestGetFinalPermissionsFromRolesAndGroups()
        {
            try
            {
                _fixture.OpenTransaction();
                // Permission 1, Role 1, Permission 2, Permission 3, Group 1, Group 2

                // Link Permission 1 to Role 1, RW

                // Link Permission 2 to Group 1, RW

                // Link Permission 3 to Group 2, RW

                // Link Role 1 and Group 1 to user 1

                // TODO write the test code, have a deeper look at finding repositories and getting them initialized with an EF Core DbContext reference
            }
            finally
            {
                _fixture.RollbackTransaction();
            }
        }

        [Fact]
        public void TestGetFinalPermissionsWithLevel()
        {
            try
            {
                _fixture.OpenTransaction();
                // Permission 1, Role 1, User 1, Group 1

                // Link Permission 1 to Role 1, RO

                // Link Permission 1 to User 1, Never

                // Link Permission 1 to Group 1, RW

                // Link Role 1 and Group 1 to user 1

                // TODO write the test code, have a deeper look at finding repositories and getting them initialized with an EF Core DbContext reference
            }
            finally
            {
                _fixture.RollbackTransaction();
            }
        }

    }
}
