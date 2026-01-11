using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Payspace.Utilities
{
    public class PasswordFunctions
    {
        public static string GetSHA256(string data)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));
                return string.Join("", bytes.Select(x => x.ToString("x2")));
            }
        }
        public static bool IsMatchingHash(string data, string hash)
        {
            return GetSHA256(data) == hash;
        }
    }
}
