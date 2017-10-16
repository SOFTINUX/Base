using Security;
using SecurityTest.Util;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class UserManagerTest : BaseTest
    {
        public UserManagerTest(DatabaseFixture fixture_)
        {
            _fixture = fixture_;

        }

        [Fact]
        public void TestNoCredentialType()
        {
           // no need of transaction (no db write)
            Assert.Null(new UserManager(_fixture.DatabaseContext, ((TestContext)_fixture.DatabaseContext).LoggerFactory)
                .Login("wrong", "user", "123password"));

        }
    }
}
