// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace SampleExtension2
{
    /// <summary>
    /// Enum values are decorated with Display attribute and have short value.
    /// </summary>
    public enum SamplePermissions2
    {
        [Display(GroupName = "Sample", Name = "Read", Description = "Can read data")]
        Read = 50,
        [Display(GroupName = "Sample", Name = "Write", Description = "Can write data")]
        Write = 51,
        [Display(GroupName = "Sample", Name = "Admin", Description = "Is admin")]
        Admin = 52,
        Other = 53
    }
}