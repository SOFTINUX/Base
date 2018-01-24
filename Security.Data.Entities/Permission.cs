// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;
using Security.Common;

namespace Security.Data.Entities
{
    public class Permission : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Full name of extension's assembly, to manage data by extension (add, reset, remove).
        /// </summary>
        public string OriginExtension { get; set; }

        /// <summary>
        /// Unique identifier : code + origin extension name.
        /// </summary>
        public string UniqueIdentifier => PolicyUtil.GetPermissionUniqueIdentifier(Name, OriginExtension);

    }
}
