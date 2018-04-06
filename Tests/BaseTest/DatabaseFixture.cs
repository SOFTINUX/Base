using BaseTest.Common;

namespace BaseTest
{
    /// <summary>
    /// Implementation of DatabaseFixture that uses a custom TestContext that uses a SQLite ExtCore storage context.
    /// </summary>
    public class DatabaseFixture : BaseTest.Common.DatabaseFixture
    {
        public override ITestContext GetTestContext()
        {
            return new TestContext(ConnectionStringPath);
        }
    }
}
