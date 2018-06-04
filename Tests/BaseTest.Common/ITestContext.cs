using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BaseTest.Common
{
    /// <summary>
    /// Test context interface: a test context should define:
    /// - an ExtCore storage context implementation (IStorageContext), specific to a provider
    /// - an IdentityDbContext implementation, thanks to a DbContextOptionsBuilder (usqes options specific to a provider).
    /// </summary>
    public interface ITestContext
    {
        IStorageContext GetProviderStorageContext(DbContextOptions<BaseTestDbContext> options_);

        DbContextOptionsBuilder<BaseTestDbContext> GetDbContextOptionsBuilder();
    }
}
