// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;

namespace Infrastructure
{
    /// <summary>
    /// Implementing this interface allows your extension to provide permissions, roles and groups
    /// that you wish to record to database.
    /// Configuring links related to these elements (attributing permissions to roles etc)
    /// will be done using Security's administration interface or through custom code in ConfigureLinks(IStorage) implementation,
    /// using storage_.GetRepository<IYourRepository>();
    /// </summary>
    public interface IExtensionDatabaseMetadata
    {
        /// <summary>
        /// Useful if an extension depends on another one's permissions, roles...
        /// For example Security's priority is 0 (the lowest value).
        /// </summary>
        /// <returns></returns>
        uint Priority { get; }

        /// <summary>
        /// The permissions you wish to add to the application with code (unique to your extension), label, administrator-owner flag.
        /// You will use this code to use Security.Common.AuthorizeAttribute.
        /// Label is useful to distinguish this item among others in administration interface.
        /// When administrator-owner flag is set, this permission will be sticky to administrator-owner role, regardless current configuration.
        /// </summary>
        IEnumerable<Tuple<string, string, bool>> PermissionCodeLabelAndFlags { get; }

        /// <summary>
        /// The roles you wish to add to the application, with code (unique to your extension) and label.
        /// Label is useful to distinguish this item among others in administration interface.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> RoleCodeAndLabels { get; }

        /// <summary>
        /// The groups you wish to add to the application, with code (unique to your extension) and label.
        /// Label is useful to distinguish this item among others in administration interface.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> GroupCodeAndLabels { get; }

        /// <summary>
        /// Your custom code to add links between permission, role, user and group repository, but you have to manage yourself
        /// whether the links already exist, and don't forget to call storage_.Save() when you're done.
        /// </summary>
        /// <param name="storage_"></param>
        void ConfigureLinks(IStorage storage_);
    }
}
