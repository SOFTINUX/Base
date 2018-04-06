using ExtCore.Data.EntityFramework;
using Microsoft.Extensions.Options;

namespace BaseTest.Common
{
    /// <summary>
    /// Test context interface: a test context should define an ExtCore storage context implementation.
    /// </summary>
    public interface ITestContext
    {
        StorageContextBase GetProviderStorageContext(IOptions<StorageContextOptions> options_);
    }
}
