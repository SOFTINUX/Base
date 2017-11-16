// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    public class Group : IEntity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Label { get; set; }

        /// <summary>
        /// Full name of extension's assembly, to manage data by extension (add, reset, remove).
        /// </summary>
        public string OriginExtension { get; set; }
    }
}
