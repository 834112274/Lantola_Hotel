using System;
using System.Security.Cryptography;
using System.Text;

namespace HotelSystem.Helper
{
    public class Encrypt
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strSource">需要加密的字符串</param>
        /// <returns>MD5加密后的字符串</returns>
        public static string Md5(string strSource)
        {
            byte[] result = Encoding.Default.GetBytes(strSource);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }
    }
}