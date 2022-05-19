using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Service_Frm
{
    public static class EncryptUtil
    {
        /// <summary>
        /// 加密算法HmacSHA256
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="signTimestampKey"></param>
        /// <returns></returns>
        public static string HmacSHA256(string secret, string signTimestampKey)
        {
            string signRet = string.Empty;
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(signTimestampKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(secret));

                signRet = ToHexString(hashBytes);
            }

            return signRet;
        }

        /// <summary>
        /// byte[]转16进制格式string
        /// </summary>
        /// <param name="hashBytes"></param>
        /// <returns></returns>
        private static string ToHexString(byte[] hashBytes)
        {
            string hexStr = string.Empty;
            if (hashBytes != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.AppendFormat("{0:x2}", b);
                }

                hexStr = sb.ToString();
            }

            return hexStr;
        }
    }
}
