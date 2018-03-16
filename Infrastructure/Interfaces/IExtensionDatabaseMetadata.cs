// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace Infrastructure.Interfaces
{
    /// <summary>
    /// Implementing this interface allows your extension to provide all the base entities, i.e. permissions, roles
    /// that you wish to record to database.
    /// After these entities have been recorded, configuring links related to these elements (attributing permissions to roles etc)
    /// will be done using Security's administration interface or through custom code in ConfigureLinks(IStorage) implementation,
#pragma warning disable 1570
    /// using storage_.GetRepository<IYourRepository>().
#pragma warning restore 1570
    /// </summary>
    public interface IExtensionDatabaseMetadata
    {
        /// <summary>
        /// Useful if an extension depends on another one's permissions, roles...
        /// For example Security's priority is 0 (the lowest value).
        /// </summary>
        /// <returns></returns>
        uint Priority { get; }

    }
}
