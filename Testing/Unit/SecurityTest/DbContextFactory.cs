using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CommonTest;

namespace SecurityTest
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationStorageContext>
    {
        public ApplicationStorageContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationStorageContext>();
            // Configure connection string
            optionsBuilder.UseSqlite("Data Source=basedb_tests.sqlite");

            return new ApplicationStorageContext(optionsBuilder.Options);
        }
    }
}