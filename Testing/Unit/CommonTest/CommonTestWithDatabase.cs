using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CommonTest
{
    /// <summary>
    /// Base class for all test classes that should use a database.
    /// A derived class should be decorated with "[Collection("Database collection")]", using the DatabaseCollection class defined in its assembly.
    /// </summary>
    public class CommonTestWithDatabase
    {
        public CommonTestWithDatabase(DatabaseFixture databaseFixture_)
        {
            this.DatabaseFixture = databaseFixture_;
        }
        protected DatabaseFixture DatabaseFixture { get; set; }

        /// <summary>
        /// Utility method to start an EF Core transaction
        /// </summary>
        public void BeginTransaction()
        {
            ((DbContext)DatabaseFixture.Storage.StorageContext).Database.BeginTransaction();
        }

        /// <summary>
        /// Utility method to commit an EF Core transaction
        /// </summary>
        public void CommitTransaction()
        {
            ((DbContext)DatabaseFixture.Storage.StorageContext).Database.CommitTransaction();
        }

        /// <summary>
        /// Utility method to rollback an EF Core transaction
        /// </summary>
        public void RollbackTransaction()
        {
            ((DbContext)DatabaseFixture.Storage.StorageContext).Database.RollbackTransaction();
        }

    }
}