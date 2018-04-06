// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace BaseTest.Util
{
    /// <summary>
    /// Another database fixture that copies another database to run tests with.
    /// This database is empty, it contains only the tables but no data in them.
    /// </summary>
    public class EmptyDatabaseFixture : DatabaseFixture
    {
        protected override string DatabaseToCopyFromBaseName => "basedb_empty";
        protected override string DatabaseToCopyToBaseName => "basedb_empty_tests";
        protected override string ConnectionStringPath => "ConnectionStrings:EmptyDb";

        public EmptyDatabaseFixture()
        {
        }
    }
}
