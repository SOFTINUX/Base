// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;

namespace Infrastructure
{
    /// <summary>
    /// Implementing this interface allows your extension to provide all the base entities, i.e. permissions, roles, groups
    /// that you wish to record to database.
    /// After these entities have been recorded, configuring links related to these elements (attributing permissions to roles etc)
    /// will be done using Security's administration interface or through custom code in ConfigureLinks(IStorage) implementation,
    /// using storage_.GetRepository<IYourRepository>().
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
        /// The credential types (way of authenticating)  you wish to add to the application, with code (unique to your extension) and label.
        /// Label is useful to distinguish this item among others in administration interface.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> CredentialTypeCodeAndLabels { get; }

        /// <summary>
        /// The permission levels that will be created in application (only by Security extension).
        /// </summary>
        IEnumerable<Tuple<int, string, string, string>> PermissionLevelIdValueLabelAndTips { get; }

        /// <summary>
        /// The users you wish to add to the application.
        /// </summary>
        IEnumerable<Tuple<string, string, string>> UserFirstnameLastnameAndDisplayNames { get; }

        /// <summary>
        /// Your custom code to add links between permission, role, user and group repository, but you have to manage yourself
        /// whether the links already exist. Don't call storage_.Save() because other extensions will insert entities of same type,
        /// and EF Core needs to commit only when they're all added else tracking fails.
        /// </summary>
        /// <param name="storage_"></param>
        void ConfigureLinks(IStorage storage_);
    }
}
