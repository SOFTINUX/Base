using System;
using System.Security.Cryptography;
using System.Text;

namespace Security
{
    public static class MD5Hasher
  {
    public static string ComputeHash(string data)
    {
      return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(data)));
    }
  }
}