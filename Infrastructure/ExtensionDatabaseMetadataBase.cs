using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;

namespace Infrastructure
{
    /// <summary>
    /// Useful base class for implementations of IExtensionDatabaseMetadata in case your extension provides only
    /// custom permissions, roles and groups, but not credential types etc.
    /// </summary>
    public abstract class ExtensionDatabaseMetadataBase : IExtensionDatabaseMetadata
    {
        public abstract uint Priority { get; }
        public abstract IEnumerable<Tuple<string, string, bool>> PermissionCodeLabelAndFlags { get; }
        public abstract IEnumerable<KeyValuePair<string, string>> RoleCodeAndLabels { get; }
        public abstract IEnumerable<KeyValuePair<string, string>> GroupCodeAndLabels { get; }

        public virtual IEnumerable<KeyValuePair<string, string>> CredentialTypeCodeAndLabels => null;
        public virtual IEnumerable<Tuple<int, string, string, string>> PermissionLevelIdValueLabelAndTips => null;
        public virtual IEnumerable<Tuple<string, string, string>> UserFirstnameLastnameAndDisplayNames => null;

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="storage_"></param>
        public virtual void ConfigureLinks(IStorage storage_)
        {

        }
    }
}
