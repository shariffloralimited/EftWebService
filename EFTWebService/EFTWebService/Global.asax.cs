using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;

namespace EFTN
{
    public class Global : HttpApplication
    {
        public static bool cancontinue;
        public class AppVariable
        {
            public static string ServerLogin = UCBLEFTWebService.ConnectionStringDecryptor.Decrypt(ConfigurationManager.AppSettings["ConnectionString"]);
            //public static string ServerLogin = "server=" + ConfigurationManager.AppSettings["DBServer"] + ";database=EFT_farm;uid=eftuser;pwd=eftuser321";
            //public static string ServerLogin = ConfigurationManager.AppSettings["DBServer"];

        }
        void Application_Start(object sender, EventArgs e)
        {
            //try
            //{
            //    string windir = Environment.GetEnvironmentVariable("windir");
            //    if (!Directory.Exists(windir + "\\Microsoft.NET\\Framework64\\v1.0"))
            //    {
            //        cancontinue = false;
            //    }
            //    else
            //    {
            //        cancontinue = true;
            //    }
            //}
            //catch
            //{
            //    cancontinue = false;
            //}
        }
    }
}