using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CommonTest;
using Xunit;

namespace CommonTest
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationStorageContext>
    {
        public ApplicationStorageContext CreateDbContext(string[] args)
        {
            return new ApplicationStorageContext(DatabaseFixture.GetDbContextOptionsBuilder().Options);
        }
    }
}