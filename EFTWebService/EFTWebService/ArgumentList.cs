using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UCBLEFTWebService
{

     public class ucblArgument
    {
            public string Reason = "";
            public string SenderAccNumber = "";
            public string ReceivingBankRouting = "";
            public string BankAccNo = "";
            public int AccType = 1;
            public string Amount ="0";
            //public string Amount = "";
           
            public string ReceiverName = "";
            public string ReceiverID = "";
            public string EDRID="" ;
            //public Guid TransactionID = "bcbfb784-798f-4e9c-ad8d-090826ea07ba";
            public string TransactionCode = "";
            public int TypeOfPayment = 0;
            //public int StatusID = 1;
            
            public int CreatedBy = 0;
            public string DepartmentID = "";
            public string Checker_StatusID = "";
         

        //batch
        public string TransactionID ="";
            public int envelopID = -1;
            public string serviceClassCode = "";
			public string SECC ="";
            public int typeOfPayment = 6;
            public DateTime effectiveEntryDate;
            public string companyId = "";
            public string companyName = "";
            public string entryDesc = "";
            public int createdBy = 0; 
			public int BatchStatus= 0; 
			public string BatchType ="";
            //public int DepartmentID = 0;   
			public string DataEntryType ="";
            public string UniqueReferenceNo="";
            public string ExternalRef = "";
            public string TrnRef = "";

    }
}

