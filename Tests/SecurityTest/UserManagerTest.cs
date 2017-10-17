using Security;
using Security.Enums.Debug;
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
            UserManager userManager = new UserManager(_fixture.DatabaseContext,
                ((TestContext) _fixture.DatabaseContext).LoggerFactory);
            Assert.Null(userManager.Login("wrong credential type", "user", "123password"));
            Assert.Equal(UserManagerErrorCode.NoCredentialType, userManager.ErrorCode);

        }

        [Fact]
        public void TestNoIdentifierFound()
        {
            // no need of transaction (no db write)
            UserManager userManager = new UserManager(_fixture.DatabaseContext,
                ((TestContext)_fixture.DatabaseContext).LoggerFactory);
            Assert.Null(userManager.Login(Security.Enums.CredentialType.Email, "no such user", "123password"));
            Assert.Equal(UserManagerErrorCode.NoMatchCredentialTypeAndIdentifier, userManager.ErrorCode);

        }

        [Fact]
        public void TestWrongSecret()
        {
            // no need of transaction (no db write)
            UserManager userManager = new UserManager(_fixture.DatabaseContext,
                ((TestContext)_fixture.DatabaseContext).LoggerFactory);
            Assert.Null(userManager.Login(Security.Enums.CredentialType.Email, "user", "not the password"));
            Assert.Equal(UserManagerErrorCode.SecretVerificationFailed, userManager.ErrorCode);

        }
    }
}
