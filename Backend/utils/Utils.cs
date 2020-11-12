using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace LM.Utils
{
    public static class Crypto
    {
        
      public static string sha256_hash(string value) {
        StringBuilder Sb = new StringBuilder();

        using (SHA256 mySHA256 = SHA256.Create())            
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = mySHA256.ComputeHash(enc.GetBytes(value));

            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
      }

      public static string concatenate_sha256_hash(List<string> _list)
      {
          return sha256_hash(string.Concat(_list));
      }    
    }        
}
