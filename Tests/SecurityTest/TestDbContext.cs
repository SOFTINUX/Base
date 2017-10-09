using ExtCore.Data.EntityFramework;
using Microsoft.Extensions.Options;

namespace SecurityTest
{
    /// <summary>
    /// Use Sqlite storage context.
    /// </summary>
    internal class TestDbContext : ExtCore.Data.EntityFramework.Sqlite.StorageContext
    {
        public TestDbContext(IOptions<StorageContextOptions> options_) : base(options_)
        {
        }
    }
}
