using ExtCore.Data.EntityFramework;
using Microsoft.Extensions.Options;

namespace BaseTest
{
    /// <summary>
    /// Implementation of TestContext that uses a SQLite ExtCore storage context.
    /// </summary>
    public class TestContext : BaseTest.Common.TestContext
    {
        public TestContext(string connectionStringPath_) : base(connectionStringPath_)
        {

        }

        public override StorageContextBase GetProviderStorageContext(IOptions<StorageContextOptions> options_)
        {
            return new TestStorageContextBase(options_);
        }
    }
}