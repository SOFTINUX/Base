// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;
using Infrastructure;

namespace Security.Data.Entities
{
    public class Permission : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }
}
