using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace UCBLEFTWebService
{
    public class ConnectionStringDecryptor
    {
        public static string Decrypt(string textToEncrypt)
        {
            StringBuilder inSb = new StringBuilder(textToEncrypt);
            StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
            char c;
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                c = inSb[i];
                c = (char)(c ^ 129);
                outSb.Append(c);
            }
            return outSb.ToString();
        }
    }
}