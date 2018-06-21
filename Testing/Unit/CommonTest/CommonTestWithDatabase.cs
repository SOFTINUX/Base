using Xunit;

namespace CommonTest
{
    /// <summary>
    /// Base class for all test classes that should use a database.
    /// A derived class should be decorated with "[Collection("Database collection")]", using the DatabaseCollection class defined in its assembly.
    /// </summary>
    public class CommonTestWithDatabase
    {
        public CommonTestWithDatabase(DatabaseFixture databaseFixture_) {
            this.DatabaseFixture = databaseFixture_;
        }
        protected DatabaseFixture DatabaseFixture {get; set;}
    }
}