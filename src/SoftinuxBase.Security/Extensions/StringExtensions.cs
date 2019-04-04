// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

namespace SoftinuxBase.Security.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Extend String class to
        /// transform first letter of string in upper case.
        /// </summary>
        /// <param name="s_">string to make first letter in upper case.</param>
        /// <returns>return the string with first letter in upper case.</returns>
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