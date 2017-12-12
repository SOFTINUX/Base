// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

namespace Infrastructure.Enums
{
    /// <summary>
    /// Value of PermissionLevel records
    /// TODO replace ReadOnly and ReadWrite by "Yes".
    /// </summary>
    public enum PermissionLevelValue
    {
        Never = 1,
        No = 2,
        ReadOnly = 4,
        ReadWrite = 8
    }
}
