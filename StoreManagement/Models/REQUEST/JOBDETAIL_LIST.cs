using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class JOBDETAIL_LIST
    {
        public string JOB_ID { get; set; }
        public string JOB_ID_ENCRYPT { get; set; }
        public string LICENSE_NO { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string CUS_FULLNAME { get; set; }
        public string SUMMARY { get; set; }
        public string ACTION { get; set; }
        public string RESULT { get; set; }
        public string TRANSFER_TO { get; set; }
        public string FIX_DATE { get; set; }
        public string CLOSE_DATE { get; set; }
        public string EMAIL_CUSTOMER { get; set; }
        public string INVOICE_NO { get; set; }
        public string OWNER_ID { get; set; }
        public string EMP_FULLNAME { get; set; }
        public string CREATE_BY { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_BY { get; set; }
        public string UPDATE_DATE { get; set; }
        public string REF_HJOB_ID { get; set; }
    }
}
