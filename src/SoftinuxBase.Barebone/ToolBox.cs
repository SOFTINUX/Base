using System;
using System.Collections.Generic;
using System.Text;

namespace Barebone
{
    public static class ToolBox
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="s_"></param>
        /// <returns></returns>
        static string UppercaseFirst(string s_)
        {
            if (string.IsNullOrEmpty(s_))
            {
                return string.Empty;
            }
            char[] a = s_.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
