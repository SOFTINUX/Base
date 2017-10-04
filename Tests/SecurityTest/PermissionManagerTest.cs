using Security;
using Security.Data.Entities;
using SecurityTest.Util;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class PermissionManagerTest : BaseTest
    {
        private const string CST_PERM_CODE = "test_perm";
        private static string _assembly = typeof(PermissionManagerTest).Assembly.FullName;

        [Fact]
        public void TestGetFinalPermissionsRw(DatabaseFixture fixture_)
        {
            _fixture = fixture_;

            Permission roPerm = new Permission { Code = CST_PERM_CODE, OriginExtension = _assembly };
            Permission rwPerm = new Permission { Code = CST_PERM_CODE, OriginExtension = _assembly };
            IEnumerable<Claim> claims = new PermissionManager().GetFinalPermissions(new List<Permission> { roPerm, rwPerm });
            Assert.Equal(1, claims.Count());

            // TODO the test code
        }

    }
}
