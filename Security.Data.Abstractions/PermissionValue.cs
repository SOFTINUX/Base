// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace Security.Data.Abstractions
{
    /// <summary>
    /// Class to hold the unique id and permission level value of an attributed permission.
    /// </summary>
    public class PermissionValue
    {
        public string UniqueId { get; set; }

        public byte Level { get; set; }

        public bool AdministratorOwner { get; set; }
    }
}
