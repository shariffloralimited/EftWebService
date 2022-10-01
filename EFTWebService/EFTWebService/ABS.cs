using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;

namespace FloraEFTWebService
{
    public class ABS
    {

        public string SendAck(string Transaction_Request_ID, string Sending_Chanel, string Transaction_Date, string Sending_Amount, string Status_Code, string Status_Description)
        {
            string inputstring = GetAckSting(Transaction_Request_ID, Sending_Chanel, Transaction_Date, Sending_Amount, Status_Code, Status_Description);
            string responsestring = SendData(inputstring);
            return responsestring;
        }


        private string SendData(string inputdata)
        {
            string outputdata = "";

            string uri = ConfigurationManager.AppSettings["ChannelURI"];
            string uriusername = ConfigurationManager.AppSettings["ChannelURIUserName"];
            string uriuserpass = ConfigurationManager.AppSettings["ChannelURIUserPass"];
            WebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Method = "POST";
            req.ContentType = "application/json";

            req.Headers.Add("service_user_id", uriusername);
            req.Headers.Add("service_password", uriuserpass);
            try
            {
                StreamWriter writer = new StreamWriter(req.GetRequestStream());
                writer.Write(inputdata);
                writer.Close();

                WebResponse rsp = (HttpWebResponse)req.GetResponse();
                StreamReader rdr = new StreamReader(rsp.GetResponseStream());
                outputdata = rdr.ReadToEnd();
                rdr.Close();
                rdr.Dispose();
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }

            return outputdata;
        }
        private string GetAckSting(string Transaction_Request_ID, string Sending_Chanel, string Transaction_Date, string Sending_Amount, string Status_Code, string Status_Description)
        {
            string JSONString = "{\"Transaction_Request_ID\":\"" + Transaction_Request_ID + "\", \"Sending_Chanel\": \"" + Sending_Chanel + "\", \"Transaction_Date\": \"" + Transaction_Date + "\",\"Sending_Amount\":\"" + Sending_Amount + "\",\"Status_Code\":\"" + Status_Code + "\",\"Status_Description\":\"" + Status_Description + "\"}";
            return JSONString;
        }
        protected void WriteLog(string Msg)
        {
            string LogPath = ConfigurationManager.AppSettings["LogPath"]; 

            FileStream fs = new FileStream(LogPath + "\\" + System.DateTime.Today.ToString("yyyyMMdd") + ".log", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(System.DateTime.Now.ToString() + ": " + Msg);
            sw.Close();
            sw.Dispose();
            fs.Close();
            fs.Dispose();
        }
    }
}
