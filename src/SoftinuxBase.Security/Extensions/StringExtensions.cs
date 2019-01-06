// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Security.Extensions
{
    public static class StringExtensions
    {
        public static string UppercaseFirst(this string s_)
        {
            if (string.IsNullOrEmpty(s_))
            {
                return string.Empty;
            }

            string s = s_.ToLowerInvariant();
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}