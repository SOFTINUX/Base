using System;
using SecurityTest.Util;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class AddAuthorizationPoliciesTest : BaseTest
    {
        public AddAuthorizationPoliciesTest(DatabaseFixture fixture_)
        {
            _fixture = fixture_;
        }

        [Fact]
        public void TestExecute()
        {
            // TODO create a mock for IServiceCollection, method AddAuthorization, that stores the data in a dictionary
            // See https://github.com/Moq/moq4/
            throw new NotImplementedException();
            
        }
    }
}