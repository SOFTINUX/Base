// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Security.Cryptography;
using System.Text;

namespace SoftinuxBase.Security
{
    public static class MD5Hasher
    {
        public static string ComputeHash(string data)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(data)));
        }
    }
}