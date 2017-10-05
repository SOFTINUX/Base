using System.IO;
using Security;
using SecurityTest.Util;
using Xunit;

namespace SecurityTest
{
    [Collection("Database collection")]
    public class DatabaseInitializerTest : BaseTest
    {
        public DatabaseInitializerTest(DatabaseFixture fixture_)
        {
            _fixture = fixture_;

            // initialize test file from copy. Root dir is bin/debug/netcoreapp2.0
            File.Copy("..\\..\\..\\..\\Artefacts\\basedb_empty.sqlite", "..\\..\\..\\..\\WorkDir\\basedb_tests.sqlite", true);
        }

        [Fact]
        public void Test()
        {
            new DatabaseInitializer().CheckAndInitialize(_fixture.DatabaseContext);
        }
    }
}
