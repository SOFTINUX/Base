using System;
using System.IO;
using Infrastructure;

namespace SecurityTest.Util
{
    public class DatabaseFixture : IDisposable
    {
        public IRequestHandler DatabaseContext { get; }

        public DatabaseFixture()
        {
            // initialize test file from copy. Root dir is bin/debug/netcoreapp2.0
            File.Copy("..\\..\\..\\..\\Artefacts\\basedb.sqlite", "..\\..\\..\\..\\WorkDir\\basedb_tests.sqlite", true);

            DatabaseContext = new TestContext();
        }

        public void Dispose()
        {
            // nothing yet
        }
    }
}
