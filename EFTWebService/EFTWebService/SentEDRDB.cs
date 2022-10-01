using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;


namespace UCBLEFTWebService
{
    class SentEDRDB
    {
        //Service1 dd = new Service1();

        public string InsertTransactionBatch(ucblArgument pacs)
        {
            //Guid TransID = Guid.NewGuid();
            DateTime dtime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            

            SqlConnection myConnection = new SqlConnection(EFTN.AppVariables.ConStr);
            SqlCommand myCommand = new SqlCommand("EFT_InsertBatchSent_webService", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            //SqlParameter parameterTransactionID = new SqlParameter("@TransactionID", SqlDbType.UniqueIdentifier);
            //parameterTransactionID.Value = pacs.TransactionID;
            //myCommand.Parameters.Add(parameterTransactionID);

            SqlParameter parameterTransactionID = new SqlParameter("@TransactionID", SqlDbType.UniqueIdentifier);
            parameterTransactionID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterTransactionID);

            SqlParameter paramEnvelopID = new SqlParameter("@EnvelopID", SqlDbType.Int, 4);
            paramEnvelopID.Value = pacs.envelopID;
            myCommand.Parameters.Add(paramEnvelopID);

            SqlParameter paramServiceClassCode = new SqlParameter("@ServiceClassCode", SqlDbType.NVarChar, 3);
            paramServiceClassCode.Value = "200";
            myCommand.Parameters.Add(paramServiceClassCode);

            SqlParameter paramSECC = new SqlParameter("@SECC", SqlDbType.NVarChar, 3);
            //paramSECC.Value = "PPD";
            paramSECC.Value = pacs.SECC;

            myCommand.Parameters.Add(paramSECC);

            SqlParameter paramTypeOfPayment = new SqlParameter("@TypeOfPayment", SqlDbType.TinyInt);
            paramTypeOfPayment.Value = pacs.typeOfPayment;
            myCommand.Parameters.Add(paramTypeOfPayment);

            SqlParameter paramEffectiveEntryDate = new SqlParameter("@EffectiveEntryDate", SqlDbType.DateTime);
            paramEffectiveEntryDate.Value = dtime;
            myCommand.Parameters.Add(paramEffectiveEntryDate);

            SqlParameter paramCompanyId = new SqlParameter("@CompanyId", SqlDbType.NVarChar, 10);
            paramCompanyId.Value = pacs.companyId;
            myCommand.Parameters.Add(paramCompanyId);

            SqlParameter paramCompanyName = new SqlParameter("@CompanyName", SqlDbType.NVarChar, 16);
            paramCompanyName.Value = pacs.companyName;
            myCommand.Parameters.Add(paramCompanyName);

            SqlParameter paramEntryDesc = new SqlParameter("@EntryDesc", SqlDbType.NVarChar, 10);
            paramEntryDesc.Value = pacs.entryDesc;
            myCommand.Parameters.Add(paramEntryDesc);

            SqlParameter paramCreatedBy = new SqlParameter("@CreatedBy", SqlDbType.Int, 4);
            paramCreatedBy.Value = pacs.createdBy;
            myCommand.Parameters.Add(paramCreatedBy);

            SqlParameter parameterBatchStatus = new SqlParameter("@BatchStatus", SqlDbType.Int);
            parameterBatchStatus.Value = pacs.BatchStatus;
            myCommand.Parameters.Add(parameterBatchStatus);

            SqlParameter parameterBatchType = new SqlParameter("@BatchType", SqlDbType.NVarChar, 6);
            parameterBatchType.Value = pacs.BatchType;
            myCommand.Parameters.Add(parameterBatchType);

            SqlParameter parameterDepartmentID = new SqlParameter("@DepartmentID", SqlDbType.Int);
            parameterDepartmentID.Value = pacs.DepartmentID;
            myCommand.Parameters.Add(parameterDepartmentID);

            SqlParameter parameterDataEntryType = new SqlParameter("@DataEntryType", SqlDbType.NVarChar, 6);
            parameterDataEntryType.Value = "WEB";
            myCommand.Parameters.Add(parameterDataEntryType);

            SqlParameter parameterUniqueReferenceNo = new SqlParameter("@UniqueReferenceNo", SqlDbType.VarChar, 50);
            parameterUniqueReferenceNo.Value = pacs.UniqueReferenceNo;
            myCommand.Parameters.Add(parameterUniqueReferenceNo);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            //return (Guid)paramTransactionID.Value;          

            string tranID = parameterTransactionID.Value.ToString();


            myConnection.Close();
            myConnection.Dispose();
            myCommand.Dispose();

            return tranID;
        }

        public DataTable GetEFTNStatus(string TranID)
        {

            SqlConnection myConnection = new SqlConnection(EFTN.AppVariables.ConStr);
            SqlDataAdapter myCommand = new SqlDataAdapter("EFT_SearchTransactionStatusForUCBL", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterMsgID = new SqlParameter("@UniqueID", SqlDbType.VarChar, 50);
            parameterMsgID.Value = TranID;
            myCommand.SelectCommand.Parameters.Add(parameterMsgID);

            myConnection.Open();
            DataTable dt = new DataTable();
            myCommand.Fill(dt);

            myConnection.Close();
            myCommand.Dispose();
            myConnection.Dispose();

            return dt;

        }


        public string InsertTransactionSent(ucblArgument pacs)
        {            

            SqlConnection myConnection = new SqlConnection(EFTN.AppVariables.ConStr);
            SqlCommand myCommand = new SqlCommand("EFT_InsertTransactionSent_webService", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter parameterReason = new SqlParameter("@PaymentInfo", SqlDbType.NVarChar, 80);
            parameterReason.Value = pacs.Reason;
            myCommand.Parameters.Add(parameterReason);

            SqlParameter parameterTransactionID = new SqlParameter("@TransactionID", SqlDbType.UniqueIdentifier);
            // parameterTransactionID.Value = pacs.TransactionID;
            parameterTransactionID.Value = HttpContext.Current.Session["TranID"];
            myCommand.Parameters.Add(parameterTransactionID);


            SqlParameter parameterTransactionCode = new SqlParameter("@TransactionCode", SqlDbType.NVarChar, 3);
            parameterTransactionCode.Value = pacs.BatchType;
            myCommand.Parameters.Add(parameterTransactionCode);


            SqlParameter parameterSenderAccNumber = new SqlParameter("@AccountNo", SqlDbType.NVarChar, 16);
            parameterSenderAccNumber.Value = pacs.SenderAccNumber;
            myCommand.Parameters.Add(parameterSenderAccNumber);

            SqlParameter parameterReceivingBankRouting = new SqlParameter("@ReceivingBankRoutingNo", SqlDbType.NVarChar, 9);
            parameterReceivingBankRouting.Value = pacs.ReceivingBankRouting;
            myCommand.Parameters.Add(parameterReceivingBankRouting);


            SqlParameter parameterBankAccNo = new SqlParameter("@DFIAccountNo", SqlDbType.NVarChar, 17);
            parameterBankAccNo.Value = pacs.BankAccNo;
            myCommand.Parameters.Add(parameterBankAccNo);


            SqlParameter parameterAccType = new SqlParameter("@ReceiverAccountType", SqlDbType.TinyInt);
            parameterAccType.Value = pacs.AccType;
            myCommand.Parameters.Add(parameterAccType);

            SqlParameter parameterTypeOfPayment = new SqlParameter("@TypeOfPayment", SqlDbType.TinyInt);
            parameterTypeOfPayment.Value = pacs.TypeOfPayment;
            myCommand.Parameters.Add(parameterTypeOfPayment);

            //SqlParameter parameterStatusID = new SqlParameter("@StatusID", SqlDbType.TinyInt);
            //parameterStatusID.Value = pacs.StatusID;
            //myCommand.Parameters.Add(parameterStatusID);

            SqlParameter parameterCreatedBy = new SqlParameter("@CreatedBy", SqlDbType.TinyInt);
            parameterCreatedBy.Value = pacs.CreatedBy;
            myCommand.Parameters.Add(parameterCreatedBy);

            SqlParameter parameterDepartmentID = new SqlParameter("@DepartmentID", SqlDbType.Int);
            parameterDepartmentID.Value = pacs.DepartmentID;
            myCommand.Parameters.Add(parameterDepartmentID);


            SqlParameter parameterAmount = new SqlParameter("@Amount", SqlDbType.Money);
            parameterAmount.Value = pacs.Amount;
            myCommand.Parameters.Add(parameterAmount);


            SqlParameter parameterReceiverName = new SqlParameter("@ReceiverName", SqlDbType.NVarChar,22);
            parameterReceiverName.Value = pacs.ReceiverName;
            myCommand.Parameters.Add(parameterReceiverName);


            SqlParameter parameterReceiverID = new SqlParameter("@IdNumber", SqlDbType.NVarChar,22);
            parameterReceiverID.Value = pacs.ReceiverID;
            myCommand.Parameters.Add(parameterReceiverID);


            SqlParameter parameterEDRID = new SqlParameter("@EDRID", SqlDbType.UniqueIdentifier);
            parameterEDRID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterEDRID);


            SqlParameter parameterUniqueReferenceNo = new SqlParameter("@UniqueReferenceNo", SqlDbType.VarChar, 50);
            parameterUniqueReferenceNo.Value = pacs.UniqueReferenceNo;
            myCommand.Parameters.Add(parameterUniqueReferenceNo);

            SqlParameter parameterExternalRef = new SqlParameter("@ExternalRef", SqlDbType.VarChar, 50);
            parameterExternalRef.Value = pacs.ExternalRef;
            myCommand.Parameters.Add(parameterExternalRef);

            SqlParameter parameterTrnRef = new SqlParameter("@TrnRef", SqlDbType.VarChar, 50);
            parameterTrnRef.Value = pacs.TrnRef;
            myCommand.Parameters.Add(parameterTrnRef);

            SqlParameter parameterChecker_Status = new SqlParameter("@Checker_Status", SqlDbType.Int);
            parameterChecker_Status.Value = pacs.Checker_StatusID;
            myCommand.Parameters.Add(parameterChecker_Status);


            myConnection.Open();
            myCommand.ExecuteNonQuery();

            string edrid = parameterEDRID.Value.ToString();
            //string MsgDefIdr = "";

            myConnection.Close();
            myConnection.Dispose();
            myCommand.Dispose();

            return edrid;
        }

        public bool CheckUniqueNo(string UniqueReferenceNo)
        {

            SqlConnection myConnection = new SqlConnection(EFTN.AppVariables.ConStr);
            SqlCommand myCommand = new SqlCommand("EFT_CheckUniqueNo_webService", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter parameterUniqueReferenceNo = new SqlParameter("@UniqueReferenceNo", SqlDbType.VarChar, 50);
            parameterUniqueReferenceNo.Value = UniqueReferenceNo;
            myCommand.Parameters.Add(parameterUniqueReferenceNo);

            SqlParameter parameterisDuplicate = new SqlParameter("@isDuplicate", SqlDbType.Bit);
            parameterisDuplicate.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterisDuplicate);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            bool isDuplicate = (bool)parameterisDuplicate.Value;
            myConnection.Close();
            myConnection.Dispose();
            myCommand.Dispose();

            return isDuplicate;

       
          
        }

        public bool CheckRoutingNo(string ValidRoutingNo)
        {

            SqlConnection myConnection = new SqlConnection(EFTN.AppVariables.ConStr);
            SqlCommand myCommand = new SqlCommand("EFT_ValidRoutingNo_webService", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter parameterValidRoutingNo = new SqlParameter("@ValidRoutingNo", SqlDbType.VarChar, 50);
            parameterValidRoutingNo.Value = ValidRoutingNo;
            myCommand.Parameters.Add(parameterValidRoutingNo);

            SqlParameter parameterisDuplicate = new SqlParameter("@isExists", SqlDbType.Bit);
            parameterisDuplicate.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterisDuplicate);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            bool isExist = (bool)parameterisDuplicate.Value;
            myConnection.Close();
            myConnection.Dispose();
            myCommand.Dispose();

            return isExist;
                 
        }

        public bool CheckDepartment(string ValidDepartment)
        {

            SqlConnection myConnection = new SqlConnection(EFTN.AppVariables.ConStr);
            SqlCommand myCommand = new SqlCommand("EFT_ValidDepartment_webService", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter parameterValidDepartment = new SqlParameter("@ValidDepartment", SqlDbType.VarChar);
            parameterValidDepartment.Value =  ValidDepartment;
            myCommand.Parameters.Add(parameterValidDepartment);

            SqlParameter parameterisDuplicate = new SqlParameter("@isExists", SqlDbType.Bit);
            parameterisDuplicate.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterisDuplicate);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            bool isExist = (bool)parameterisDuplicate.Value;
            myConnection.Close();
            myConnection.Dispose();
            myCommand.Dispose();

            return isExist;

        }

        public bool CheckAccountLength(string ValidAccount)
        {

            SqlConnection myConnection = new SqlConnection(EFTN.AppVariables.ConStr);
            SqlCommand myCommand = new SqlCommand("EFT_ValidAccountLength_webService", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter parameterValidAccount = new SqlParameter("@ValidAccountNo", SqlDbType.VarChar, 17);
            parameterValidAccount.Value = ValidAccount;
            myCommand.Parameters.Add(parameterValidAccount);

            SqlParameter parameterisDuplicate = new SqlParameter("@isExists", SqlDbType.Bit);
            parameterisDuplicate.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterisDuplicate);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            bool isExist = (bool)parameterisDuplicate.Value;
            myConnection.Close();
            myConnection.Dispose();
            myCommand.Dispose();

            return isExist;

        }

        public string InsertTransactionSentTemp(ucblArgument pacs)
        {

            SqlConnection myConnection = new SqlConnection(EFTN.AppVariables.ConStr);
            //SqlCommand myCommand = new SqlCommand("EFT_InsertTransactionSent_webService", myConnection);
            SqlCommand myCommand = new SqlCommand("EFT_TempInsertTransactionSent_webService", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;


            SqlParameter parameterReason = new SqlParameter("@PaymentInfo", SqlDbType.NVarChar, 80);
            parameterReason.Value = pacs.Reason;
            myCommand.Parameters.Add(parameterReason);

            /*SqlParameter parameterTransactionID = new SqlParameter("@TransactionID", SqlDbType.UniqueIdentifier);
             parameterTransactionID.Value = pacs.TransactionID; //comment out 25.03.2020
            //parameterTransactionID.Value = HttpContext.Current.Session["TranID"]; //comment on 25.03.2020
            myCommand.Parameters.Add(parameterTransactionID);*/


            SqlParameter parameterTransactionCode = new SqlParameter("@TransactionCode", SqlDbType.NVarChar, 3);
            parameterTransactionCode.Value = pacs.BatchType;
            myCommand.Parameters.Add(parameterTransactionCode);


            SqlParameter parameterSenderAccNumber = new SqlParameter("@AccountNo", SqlDbType.NVarChar, 16);
            parameterSenderAccNumber.Value = pacs.SenderAccNumber;
            myCommand.Parameters.Add(parameterSenderAccNumber);

            SqlParameter parameterReceivingBankRouting = new SqlParameter("@ReceivingBankRoutingNo", SqlDbType.NVarChar, 9);
            parameterReceivingBankRouting.Value = pacs.ReceivingBankRouting;
            myCommand.Parameters.Add(parameterReceivingBankRouting);


            SqlParameter parameterBankAccNo = new SqlParameter("@DFIAccountNo", SqlDbType.NVarChar, 17);
            parameterBankAccNo.Value = pacs.BankAccNo;
            myCommand.Parameters.Add(parameterBankAccNo);


            SqlParameter parameterAccType = new SqlParameter("@ReceiverAccountType", SqlDbType.TinyInt);
            parameterAccType.Value = pacs.AccType;
            myCommand.Parameters.Add(parameterAccType);

            SqlParameter parameterTypeOfPayment = new SqlParameter("@TypeOfPayment", SqlDbType.TinyInt);
            parameterTypeOfPayment.Value = pacs.TypeOfPayment;
            myCommand.Parameters.Add(parameterTypeOfPayment);

            //SqlParameter parameterStatusID = new SqlParameter("@StatusID", SqlDbType.TinyInt);
            //parameterStatusID.Value = pacs.StatusID;
            //myCommand.Parameters.Add(parameterStatusID);

            SqlParameter parameterCreatedBy = new SqlParameter("@CreatedBy", SqlDbType.TinyInt);
            parameterCreatedBy.Value = pacs.CreatedBy;
            myCommand.Parameters.Add(parameterCreatedBy);

            SqlParameter parameterDepartmentID = new SqlParameter("@DepartmentID", SqlDbType.Int);
            parameterDepartmentID.Value = pacs.DepartmentID;
            myCommand.Parameters.Add(parameterDepartmentID);


            SqlParameter parameterAmount = new SqlParameter("@Amount", SqlDbType.Money);
            parameterAmount.Value = pacs.Amount;
            myCommand.Parameters.Add(parameterAmount);


            SqlParameter parameterReceiverName = new SqlParameter("@ReceiverName", SqlDbType.NVarChar, 22);
            parameterReceiverName.Value = pacs.ReceiverName;
            myCommand.Parameters.Add(parameterReceiverName);


            SqlParameter parameterReceiverID = new SqlParameter("@IdNumber", SqlDbType.NVarChar, 22);
            parameterReceiverID.Value = pacs.ReceiverID;
            myCommand.Parameters.Add(parameterReceiverID);


            SqlParameter parameterEDRID = new SqlParameter("@EDRID", SqlDbType.UniqueIdentifier);
            parameterEDRID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterEDRID);


            SqlParameter parameterUniqueReferenceNo = new SqlParameter("@UniqueReferenceNo", SqlDbType.VarChar, 50);
            parameterUniqueReferenceNo.Value = pacs.UniqueReferenceNo;
            myCommand.Parameters.Add(parameterUniqueReferenceNo);

            SqlParameter parameterExternalRef = new SqlParameter("@ExternalRef", SqlDbType.VarChar, 50);
            parameterExternalRef.Value = pacs.ExternalRef;
            myCommand.Parameters.Add(parameterExternalRef);

            SqlParameter parameterTrnRef = new SqlParameter("@TrnRef", SqlDbType.VarChar, 50);
            parameterTrnRef.Value = pacs.TrnRef;
            myCommand.Parameters.Add(parameterTrnRef);

            SqlParameter parameterChecker_Status = new SqlParameter("@Checker_Status", SqlDbType.Int);
            parameterChecker_Status.Value = pacs.Checker_StatusID;
            myCommand.Parameters.Add(parameterChecker_Status);

            //for batch 25.03.020
            SqlParameter paramSECC = new SqlParameter("@SECC", SqlDbType.NVarChar, 3);
            //paramSECC.Value = "PPD";
            paramSECC.Value = pacs.SECC;
            myCommand.Parameters.Add(paramSECC);

            SqlParameter paramCompanyId = new SqlParameter("@CompanyId", SqlDbType.NVarChar, 10);
            paramCompanyId.Value = pacs.companyId;
            myCommand.Parameters.Add(paramCompanyId);

            SqlParameter paramCompanyName = new SqlParameter("@CompanyName", SqlDbType.NVarChar, 16);
            paramCompanyName.Value = pacs.companyName;
            myCommand.Parameters.Add(paramCompanyName);

            SqlParameter paramEntryDesc = new SqlParameter("@EntryDesc", SqlDbType.NVarChar, 10);
            paramEntryDesc.Value = pacs.entryDesc;
            myCommand.Parameters.Add(paramEntryDesc);

            SqlParameter parameterBatchType = new SqlParameter("@BatchType", SqlDbType.NVarChar, 6);
            parameterBatchType.Value = pacs.BatchType;
            myCommand.Parameters.Add(parameterBatchType);


            myConnection.Open();
            myCommand.ExecuteNonQuery();

            string edrid = parameterEDRID.Value.ToString();
            //string MsgDefIdr = "";

            myConnection.Close();
            myConnection.Dispose();
            myCommand.Dispose();

            return edrid;
        }

        public DataTable GetEFTNStatusTemp(string TranID)
        {

            SqlConnection myConnection = new SqlConnection(EFTN.AppVariables.ConStr);
            //SqlDataAdapter myCommand = new SqlDataAdapter("EFT_SearchTransactionStatusForUCBL", myConnection);
            SqlDataAdapter myCommand = new SqlDataAdapter("EFT_TempSearchTransactionStatus", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterMsgID = new SqlParameter("@UniqueID", SqlDbType.VarChar, 50);
            parameterMsgID.Value = TranID;
            myCommand.SelectCommand.Parameters.Add(parameterMsgID);

            myConnection.Open();
            DataTable dt = new DataTable();
            myCommand.Fill(dt);

            myConnection.Close();
            myCommand.Dispose();
            myConnection.Dispose();

            return dt;

        }

    }
}
