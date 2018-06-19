using ExtCore.Data.EntityFramework;
using Infrastructure.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommonTest
{
    /// <summary>
    /// Provider-specific storage context to use, here Sqlite, with sensitive data logging enabled for unit tests debugging.
    /// </summary>
    public class SqliteStorageContext : ExtCore.Data.EntityFramework.Sqlite.StorageContext
    {
        public SqliteStorageContext(IOptions<StorageContextOptions> options_) : base(options_)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder_)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EfLoggerProvider());
            base.OnConfiguring(optionsBuilder_.EnableSensitiveDataLogging().UseLoggerFactory(loggerFactory));
        }
    }
}