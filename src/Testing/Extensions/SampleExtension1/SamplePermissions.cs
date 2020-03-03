// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace SampleExtension1
{
    public enum SamplePermissions
    {
        [Display(GroupName = "Sample", Name = "Read", Description = "Can read data")]
        Read,
        [Display(GroupName = "Sample", Name = "Write", Description = "Can write data")]
        Write,
        [Display(GroupName = "Sample", Name = "Admin", Description = "Is admin")]
        Admin,
        Other
    }
}