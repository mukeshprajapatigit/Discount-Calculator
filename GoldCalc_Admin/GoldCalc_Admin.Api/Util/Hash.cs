using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GoldCalc.Util
{
    public static class Hash
    {
       public static string ToHMACSHA1(this string text)
        {
            const KeyDerivationPrf Pbkdf2Prf = KeyDerivationPrf.HMACSHA1; 
            const int Pbkdf2IterCount = 1500; 
            const int Pbkdf2SubkeyLength = 512 / 8; 
            const int SaltSize = 128 / 8;
            byte[] salt = new byte[SaltSize];
            //options..GetBytes(salt);
            byte[] subkey = KeyDerivation.Pbkdf2(text, salt, Pbkdf2Prf, Pbkdf2IterCount, Pbkdf2SubkeyLength);
            var outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
            outputBytes[0] = 0x00; // format marker
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);
          
            return Convert.ToBase64String(outputBytes);
        }
    }
}
