// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Entities.Abstractions;

namespace SoftinuxBase.Security.Data.Entities
{
    public class Permission : IEntity
    {
        private string _name;
        /// <summary>
        /// Gets or sets primary key: GUID.
        /// </summary>
        /// <value></value>
        public string Id { get; set; }
        public string Name
        {
            get {
                return _name;
            }
            set
            {
                _name = value;
                NormalizedName = value.ToUpperInvariant();
            }
        }

        public string NormalizedName { get; private set; }
    }
}
