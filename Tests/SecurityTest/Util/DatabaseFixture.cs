// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ExtCore.Data.Abstractions;
using ExtCore.Data.EntityFramework;
using ExtCore.Infrastructure;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace SecurityTest.Util
{
    public class DatabaseFixture : IDisposable
    {
        public IRequestHandler DatabaseContext { get; }

        public DatabaseFixture()
        {
            // Cleanup EF detailed log file
            File.Delete(EfLoggerProvider.LogFilePath);

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
                catch (Exception)
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

        public TRepository GetRepository<TRepository>() where TRepository : IRepository
        {
            return DatabaseContext.Storage.GetRepository<TRepository>();
        }

        public void RollbackTransaction()
        {
            ((StorageContextBase)DatabaseContext.Storage.StorageContext).Database.RollbackTransaction();
            DetachAllEntities();
        }

        public void Dispose()
        {
            // nothing yet
        }

        public void DetachAllEntities()
        {
            // Since the transaction was rolled back, the entries' state is Unmodified (I guess)
            var changedEntriesCopy = ((DbContext) DatabaseContext.Storage.StorageContext).ChangeTracker.Entries()
                //.Where(e => e.State == EntityState.Added ||
                //            e.State == EntityState.Modified ||
                //            e.State == EntityState.Deleted)
                .ToList();
            foreach (var entity in changedEntriesCopy)
            {
                ((DbContext)DatabaseContext.Storage.StorageContext).Entry(entity.Entity).State = EntityState.Detached;
            }
        }
    }
}
