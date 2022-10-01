using System;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using FloraEFTWebService;

namespace UCBLEFTWebService
{
    public class AuthHeader : SoapHeader
    {
        public string Username;
        public string Password;
    }

    [WebService(Namespace = "Result_Message")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        public AuthHeader Authentication;
        public string UserName = ConfigurationManager.AppSettings["UserName"];
        public string Password = ConfigurationManager.AppSettings["Password"];
        protected string LogPath = ConfigurationManager.AppSettings["LogPath"];
        //[WebMethod]
        //public bool AreYouUp()
        //{
        //    if (Authentication.Username == UserName && Authentication.Password == Password)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        //string TranID = null;

        [WebMethod(EnableSession = true)]
        public string Initiate_EFT_Transactions
        (
           string Reason,
           string SenderAccNumber,
           string ReceivingBankRouting,
           string BankAccNo,
           string Credit_True1_False0,//TranType,
           string Amount,
           string ReceiverName,
           string ReceiverID,
           string Checker_StatusID_CBS1_False0,
           string DepartmentID,
           string Secc_1CCD_2PPD_3CIE,//----------
           string EntryDesc,
           string UniqueReferenceNo,
           string ExternalRef,
           string TrnRef

        )
        {
            UCBLEFTWebService.ucblArgument pacs = new ucblArgument();
            pacs = setPacs008(
                                 Reason,
                                 SenderAccNumber,
                                 ReceivingBankRouting,
                                 BankAccNo,
                                 Credit_True1_False0,//TranType,
                                 Amount,
                                 ReceiverName,
                                 ReceiverID,
                                 Checker_StatusID_CBS1_False0,
                                 DepartmentID,
                                 Secc_1CCD_2PPD_3CIE,//-------
                                 EntryDesc,
                                 UniqueReferenceNo,
                                 ExternalRef,
                                 TrnRef
                             );

            string errMsg = Validate(pacs);
            if (errMsg != "")
            {
                WriteLog(errMsg);
            }
            else
            {
                #region Previous
                /*//Guid TransID = Guid.NewGuid();
                UCBLEFTWebService.SentEDRDB db = new SentEDRDB();
                string TranID = db.InsertTransactionBatch(pacs);
                Guid gg = new Guid(TranID);
                Session["TranID"] = gg;
                //string text = Session["TranID"].ToString();
                string data = db.InsertTransactionSent(pacs);
                
                //string search="";
                //SentEDRDB dbb = new SentEDRDB();
                //DataTable dtt = dbb.GetEFTNStatus(TranID);
                //DataSet dss = new DataSet();
                //dss.Tables.Add(dtt);

                WriteLog("Transaction ID: " + TranID + " :Successfull");
                return "00_Transaction Received Successfully ";
                //return dss + "    " + "(Please Preserve This ID For Next Query)";*/
                #endregion

                #region New

              
                 UCBLEFTWebService.SentEDRDB db = new SentEDRDB();
               /* string TranID = db.InsertTransactionBatch(pacs);
                Guid gg = new Guid(TranID);
                Session["TranID"] = gg;

                string data = db.InsertTransactionSent(pacs);*/


                string data = db.InsertTransactionSentTemp(pacs);

                WriteLog("Transaction ID: " + data + " :Successfull");
                 return "00_Transaction Received Successfully ";
                 //return dss + "    " + "(Please Preserve This ID For Next Query)";

                #endregion
            }

            return errMsg;
        }


        [WebMethod(EnableSession = true)]
        public DataSet Search_Transaction_Status(string search)
        {
            #region Previous_Search

            /*SentEDRDB db = new SentEDRDB();
            DataTable dt = db.GetEFTNStatus(search);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            WriteLog("Search for the transactionid- " + search);
            return ds;*/

            #endregion

            #region Now_Search

            SentEDRDB db = new SentEDRDB();
            DataTable dt = db.GetEFTNStatusTemp(search);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            WriteLog("Search for the transactionid- " + search);
            return ds;

            #endregion
        }

        private ucblArgument setPacs008
        (
            string Reason,
            string SenderAccNumber,
            string ReceivingBankRouting,
            string BankAccNo,
            //string AccType,
            string BatchType,
            string Amount,

            string ReceiverName,
            string ReceiverID,
            string Checker_StatusID,
            string DepartmentID,
            string secc,
            string EntryDesc,
            string UniqueReferenceNo,
            string ExternalRef="",
            string TrnRef=""

        )
        {
            UCBLEFTWebService.ucblArgument pacs = new ucblArgument();
            string Credt = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            pacs.Reason = RemoveSpecialChars(Reason);
            pacs.SenderAccNumber = SenderAccNumber;
            pacs.ReceivingBankRouting = ReceivingBankRouting;
            pacs.BankAccNo = BankAccNo;
            pacs.BatchType = BatchType;
            pacs.Amount = Amount;
            pacs.ReceiverName = RemoveSpecialChars(ReceiverName);
            pacs.ReceiverID = ReceiverID;
            pacs.DepartmentID = DepartmentID;
            pacs.SECC = secc;
            pacs.entryDesc = EntryDesc;
            pacs.Checker_StatusID = Checker_StatusID;
            pacs.UniqueReferenceNo = UniqueReferenceNo;
            pacs.ExternalRef = ExternalRef;
            pacs.TrnRef = TrnRef;

            return pacs;

        }

        //string ReturnedString = RemoveSpecialChars(str);

        //private string GetAcknowledge() 
        //{
        //    ABS ab = new ABS();
        //    DataTable dt=ab.SendAck(......................);
            
        //    return "";
        
        //}

        private string Validate(ucblArgument pacs)
        {
            SentEDRDB db=new SentEDRDB();
            string errmsg = "";

            if (pacs.Reason == "")
            {

                errmsg = errmsg + "\n" + "01_Reason Field is Empty. ";
            }

            if (pacs.SenderAccNumber == "")
            {

                errmsg = errmsg + "\n" + "02_SenderAccNumber Field is Empty. ";
            }


            if (pacs.SenderAccNumber == "")
            {

                errmsg = errmsg + "\n" + "02_SenderAccNumber Field is Empty. ";
            }




            if (pacs.ReceivingBankRouting == "")
            {

                errmsg = errmsg + "\n" + "03_ReceivingBankRouting Field is Empty. ";
            }
            else
            {
                Regex nonNumericRegex = new Regex(@"\D");
                if (nonNumericRegex.IsMatch(pacs.ReceivingBankRouting))
                {
                    errmsg = errmsg + "\n" + "04_Use only numeric for ReceivingBankRouting field.";
                }
            }


            if (pacs.BankAccNo == "")
            {

                errmsg = errmsg + "\n" + "05_BankAccNo Field is Empty. ";
            }


            if (pacs.BatchType == "")
            {

                errmsg = errmsg + "\n" + "19_Credit/Debit Field is Empty.Use 1 for Credit and 0 for Debit. ";
            }
            else
            {
                Regex nonNumericRegex = new Regex(@"\D");
                if (nonNumericRegex.IsMatch(pacs.BatchType))
                {
                    errmsg = errmsg + "\n" + "20_Use only numeric for AccType field.Use 1 for Credit and 0 for Debit. ";
                    
                }
                else
                {
                    //var condCheck1 = new string[] { "0", "1" };
                    //if (!condCheck1.Contains(pacs.BatchType) 
                    //{
                    //    errmsg = errmsg + "\n" + "21_Use 1 for Credit and 0 for Debit. ";
                    //}

                    //if (pacs.BatchType != "0" || pacs.BatchType != "1")
                    if (pacs.BatchType != "1" && pacs.BatchType != "0")
                    {
                    errmsg = errmsg + "\n" + "21_Use 1 for Credit and 0 for Debit. ";
                    }
                }                

            }

            if (pacs.Amount == "")
            {
                errmsg = errmsg + "\n" + "22_Amount Field is Empty. ";
            }
            else
            {

                //if (pacs.Amount <= 0)
                //if (amt <= 0)
                //     {

                //     errmsg = errmsg + "\n" + "06_Use amount greater than 0. ";
                // }
                // else
                // {

                Regex r = new Regex("[a-zA-Z]+$");
                //if (pacs.Amount.Equals(r))
                if (r.IsMatch(pacs.Amount))
                {
                    errmsg = errmsg + "\n" + "07_Use only numeric value for amount.";
                }
                //Regex rr = new Regex(@"[a-zA-Z0-9_]+$");
                ////if (pacs.Amount.Equals(rr))
                //if (rr.IsMatch(pacs.Amount))
                //{
                //    errmsg = errmsg + "\n" + "08_Use only numeric value  for amount.";
                //}
                else
                {
                    decimal amt = decimal.Parse(pacs.Amount);
                    if (amt <= 0)
                    {

                        errmsg = errmsg + "\n" + "06_Use amount greater than 0. ";
                    }
                }

            }

            if (pacs.ReceiverName == "")
            {
                errmsg = errmsg + "\n" + "09_ReceiverName Field is Empty. ";
            }

            if (pacs.ReceiverID == "")
            {
                errmsg = errmsg + "\n" + "10_ReceiverID Field is Empty. ";
            }
            
            //if (pacs.Checker_StatusID == null)
            //{
            //    errmsg = errmsg + "\n" + "11_Checker_StatusID Field is Empty. ";
            //}

            if (string.IsNullOrEmpty(pacs.Checker_StatusID))
            {
                errmsg = errmsg + "\n" + "11_Checker_StatusID Field is Empty. ";
            }

            //if (string.IsNullOrEmpty(pacs.Checker_StatusID))
            //{
            //    throw new ArgumentNullException("Checker_StatusID");              
            //}

            if (pacs.UniqueReferenceNo=="")
            {
                errmsg = errmsg + "\n" + "12_Unique Reference Field is Empty. ";                
            }

            if (db.CheckUniqueNo(pacs.UniqueReferenceNo))
            {
                errmsg = errmsg + "\n" + "13_Duplicate Unique Reference No. ";
            }


            if (!db.CheckRoutingNo(pacs.ReceivingBankRouting))
            {
                errmsg = errmsg + "\n" + "14_Receiving Bank Routing No is not valid.";              
            }

            if (string.IsNullOrEmpty(pacs.DepartmentID))
            {
                errmsg = errmsg + "\n" + "15_Department Field is Empty.";
            }

       
            if (pacs.BankAccNo.Length>17)
            {
                errmsg = errmsg + "\n" + "16_Receiver Account length is greater than 17 digit.";
            }

            if (pacs.SenderAccNumber.Length > 17)
            {
                errmsg = errmsg + "\n" + "17_Sender Account length is greater than 17 digit.";
            }


            if (!db.CheckDepartment(pacs.DepartmentID))
            {
                errmsg = errmsg + "\n" + "18_Department is not valid.";
            }

            return errmsg;
        }

        public static string RemoveSpecialChars(string str)
        {
            // string array and special characters you want to remove
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]","{","}","~","`","<",">","?" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "");
                }
            }
            return str;
        }

        static string GetIntegerDigitCountString(string value)
        {
            int total = value.Length;
            string conTotal = Convert.ToString(total);
            return conTotal;
        }

        //public static string CheckAccountLength(string str)
        //{
        //    long blah = 20948230498204;
        //    for (int i = 0; i < chars.Length; i++)
        //    {
        //        if (str.Contains(chars[i]))
        //        {
        //            str = str.Replace(chars[i], "");
        //        }
        //    }
            
        //    return str;
        //}


        //private string Check(ucblArgument pacs) 
        //{

        //    SentEDRDB sa = new SentEDRDB();
        //    DataTable dt = new DataTable();
        //    dt = sa.CheckUniqueNo(pacs.UniqueReferenceNo);
        //    return dt; 
        //}

        protected void WriteLog(string Msg)
        {
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

