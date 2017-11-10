﻿// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// - 1: Never: no right, and cannot be overriden by right inheritance algorithm
    /// - 2: No: no right, can be overriden by right inheritance algorithm
    /// - 4: Read-only: Read-only right, can be overriden by right inheritance algorithm
    /// - 8: Modification: Read and modification right, cannot be overriden by right inheritance algorithm
    ///
    /// Computed permission level:
    /// - even number: no right
    /// - dividable by 8 : modification right
    /// - divisable by 4: read-only right
    /// - other: no right
    /// </summary>
    public class PermissionLevel : IEntity
    {
        public int Id { get; set; }

        public byte Value { get; set; }

        public string Label { get; set; }

        public string Tip { get; set; }
    }
}
