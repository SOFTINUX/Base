using System;
using ExtCore.Data.EntityFramework;
using Infrastructure;

namespace Security
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        public void CheckAndInitialize(IRequestHandler context_)
        {
            bool didCreate = (context_.Storage.StorageContext as StorageContextBase).Database.EnsureCreated();
            Console.WriteLine("Needed to create DB? " + didCreate);
        }
    }
}
