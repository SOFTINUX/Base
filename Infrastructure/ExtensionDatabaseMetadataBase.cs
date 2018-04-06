// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using Infrastructure.Interfaces;

namespace Infrastructure
{
    /// <summary>
    /// Useful base class for implementations of IExtensionDatabaseMetadata in case your extension provides only
    /// custom permissions, roles, but not credential types etc.
    /// </summary>
    public abstract class ExtensionDatabaseMetadataBase : IExtensionDatabaseMetadata
    {
        public abstract uint Priority { get; }
    }
}
