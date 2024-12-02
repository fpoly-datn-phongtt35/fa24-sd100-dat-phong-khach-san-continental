using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class CommonHelpers
    {

        //Giải mã
        public static string Decode(string strString, string strKeyPhrase)
        {
            try
            {
                Byte[] byt = Convert.FromBase64String(strString);
                strString = System.Text.Encoding.UTF8.GetString(byt);
                strString = KeyED(strString, strKeyPhrase);
                return strString;
            }
            catch (Exception ex)
            {

                return strString;
            }
        }

        //Mã hóa
        public static string Encode(string strString, string strKeyPhrase)
        {
            try
            {
                strString = KeyED(strString, strKeyPhrase);
                Byte[] byt = System.Text.Encoding.UTF8.GetBytes(strString);
                strString = Convert.ToBase64String(byt);
                return strString;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
        private static string KeyED(string strString, string strKeyphrase)
        {
            int strStringLength = strString.Length;
            int strKeyPhraseLength = strKeyphrase.Length;

            System.Text.StringBuilder builder = new System.Text.StringBuilder(strString);

            for (int i = 0; i < strStringLength; i++)
            {
                int pos = i % strKeyPhraseLength;
                int xorCurrPos = (int)(strString[i]) ^ (int)(strKeyphrase[pos]);
                builder[i] = Convert.ToChar(xorCurrPos);
            }

            return builder.ToString();
        }
        private static readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random _random = new Random();

        public string GeneratePassword(int length = 6)
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = _chars[_random.Next(_chars.Length)];
            }

            return new string(result);
        }
    }
}
