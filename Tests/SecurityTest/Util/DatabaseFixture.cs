using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ExtCore.Data.EntityFramework;
using ExtCore.Infrastructure;
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
            List<Assembly> loadedAssemblies = new List<Assembly>();

            foreach (FileInfo file in new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .GetFiles("*.dll")) //loop through all dll files in directory
            {
                try
                {
                    loadedAssemblies.Add(Assembly.Load(AssemblyName.GetAssemblyName(file.FullName)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading assembly from file: " + file.FullName);
                }
                }
            ExtensionManager.SetAssemblies(loadedAssemblies);
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
