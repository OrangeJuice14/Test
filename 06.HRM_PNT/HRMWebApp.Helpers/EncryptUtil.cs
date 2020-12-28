using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.Helpers
{
    public class EncryptUtil
    {
        private static String Mix(String source)
        {
            Char[] charsArrayFull = source.ToCharArray();
            Char[] charsArrayFullAfterMix = new char[charsArrayFull.Length];
            int maxIndex = charsArrayFull.Length - 1;
            int viTriLeSauCung;

            if (maxIndex / (decimal)2 != Math.Ceiling(maxIndex / (decimal)2))
            {
                viTriLeSauCung = maxIndex;
            }
            else
            {
                viTriLeSauCung = maxIndex - 1;
            }

            for (int i = 0; i < charsArrayFull.Length; i++)
            {
                if (i / (decimal)2 == Math.Ceiling(i / (decimal)2))
                {
                    charsArrayFullAfterMix[i] = charsArrayFull[i];
                }
                else
                {
                    charsArrayFullAfterMix[i] = charsArrayFull[viTriLeSauCung + 1 - i];
                }
            }
            return new String(charsArrayFullAfterMix);
        }

        public static string MD5Mix(string data)
        {
            string shortMD5 = MD5(data);
            shortMD5 = Mix(shortMD5);
            return shortMD5;
        }

        public static string MD5(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            string shortMD5 = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return shortMD5;
        }


        public static string MakeToken(string publicKey, string secretKey)
        {
            return EncryptUtil.MD5Mix(publicKey ?? "" + secretKey ?? "");
        }

    }
}