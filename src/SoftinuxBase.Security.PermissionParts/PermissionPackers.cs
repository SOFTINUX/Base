// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX.
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftinuxBase.Security.PermissionParts
{
    [Obsolete("code moved in other classes")]
    public static class PermissionPackers
    {
        // to be removed
        [Obsolete]
        public static string PackPermissions(this IEnumerable<Permissions> permissions_)
        {
            return permissions_.Aggregate("", (s, permission) => s + (char)permission);
        }

     }
}
