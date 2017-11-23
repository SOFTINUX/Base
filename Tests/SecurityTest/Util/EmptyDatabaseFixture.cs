namespace SecurityTest.Util
{
    /// <summary>
    /// Another database fixture that copies another database to run tests with.
    /// This database is empty, it contains only the tables but no data in them.
    /// </summary>
    public class EmptyDatabaseFixture : DatabaseFixture
    {
        protected override string DatabaseToCopyFromBaseName => "basedb_empty";

        public EmptyDatabaseFixture()
        {
        }
    }
}
