using System;
using System.IO;
using ExtCore.Data.EntityFramework;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

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

        public IDbContextTransaction OpenTransaction()
        {
            return ((StorageContextBase) DatabaseContext.Storage.StorageContext).Database.BeginTransaction();
        }

        public void SaveChanges()
        {
            DatabaseContext.Storage.Save();
        }

        public void RollbackTransaction()
        {
            ((StorageContextBase)DatabaseContext.Storage.StorageContext).Database.RollbackTransaction();
        }

        public void Dispose()
        {
            // nothing yet
        }
    }
}
