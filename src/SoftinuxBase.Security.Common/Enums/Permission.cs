// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;

namespace SoftinuxBase.Security.Common.Enums
{
    [Obsolete]
    public enum Permission
    {
        /// <summary>
        /// No permission.
        /// </summary>
        Never = 0,

        /// <summary>
        /// Read only access permission.
        /// </summary>
        Read = 1,

        /// <summary>
        /// Write access permission.
        /// </summary>
        Write = 2,

        /// <summary>
        /// Highest access permission.
        /// </summary>
        Admin = 4
    }
}
