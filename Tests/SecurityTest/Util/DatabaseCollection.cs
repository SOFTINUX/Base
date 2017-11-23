// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using Xunit;

namespace SecurityTest.Util
{
    /// <summary>
    /// "Markup" class that allow to use class decoration to apply an xUnit collection fixture, i.e. some code that runs
    /// before all the launched tests run and after all they end.
    ///
    /// (xUnit doc) This class has no code, and is never created. Its purpose is simply
    /// to be the place to apply [CollectionDefinition] and all the
    /// ICollectionFixture<> interfaces.
    /// </summary>
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {

    }

}
