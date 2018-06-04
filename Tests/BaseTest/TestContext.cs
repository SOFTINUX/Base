using BaseTest.Common;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
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

        public override IStorageContext GetProviderStorageContext(DbContextOptions<BaseTestDbContext> options_)
        {
            return new BaseTestDbContext(options_);
        }
        /// <summary>
        /// Configure a builder to use Sqlite provider.
        /// </summary>
        /// <returns></returns>
        public override DbContextOptionsBuilder<BaseTestDbContext> GetDbContextOptionsBuilder()
        {
            var builder = new DbContextOptionsBuilder<BaseTestDbContext>();
            builder.UseSqlite(_connectionString);
            return builder;
        }
    }
}