using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMWeb_Service.Utils;

namespace HRMWeb_Service
{
    public class Helper
    {
        public static bool TrustTest(string publicKey, string token)
        {
            try
            {
                String secretKey = "pscvietnam@hoasua";
                if (token != MakeToken(publicKey, secretKey))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private static string MakeToken(string publicKey, string secretKey)
        {
            return EncryptUtil.MD5Mix(publicKey ?? "" + secretKey ?? "");
        }
    }
}