using SecurityTest.Util;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class PermissionManagerTest : BaseTest
    {
        [Fact]
        public void TestRoRw(DatabaseFixture fixture_)
        {
            _fixture = fixture_;

            // TODO the test code
        }

    }
}
