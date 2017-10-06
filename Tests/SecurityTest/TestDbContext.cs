using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SecurityTest
{
    internal class TestDbContext : StorageContextBase
    {
        // looking at RepositoryBase in ExtCore.Data.EntiotyFramework, should be a StorageContextBase and have a Set<TEntity>

        public TestDbContext(IOptions<StorageContextOptions> options_) : base(options_)
        {
        }

        internal static IConfigurationRoot Configuration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder_)
        {
            // TODO explicitely call Security's Entity registrar 
            base.OnModelCreating(modelBuilder_);
            this.RegisterEntities(modelBuilder_);
        }

    }
}
