using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class REPORT_RESPONSE_JOB
    {
        public string JOB_DATE_FROM { get; set; }
        public string JOB_DATE_TO { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string USER_ID { get; set; }
        public string FIX_DATE_FROM { get; set; }
        public string FIX_DATE_TO { get; set; }
        public string CLOSE_DT_FROM { get; set; }

        public string CLOSE_DT_TO { get; set; }
        public string LICENSE_NO { get; set; }
        public string USER_PRINT { get; set; }
        public string TYPE_JOB { get; set; }
        public string TEACHNICIAL { get; set; }
        public string REPORT_TYPE { get; set; }
        public string DOWNTIME_DAY { get; set; }
        public string DOWNTIME_HOUR { get; set; }

        public List<JOB_LIST> SUMMARY_JOB_LIST { get; set; }
    }

    public class JOB_LIST {
        public string JOB_ID { get; set; }
        public string JOB_ID_ENCRYPT { get; set; }
        public string CREATE_DATE { get; set; }
        public string LICENSE_NO { get; set; }
        public string CREATE_BY { get; set; }
        public string EMPLOYEENAME { get; set; }
        public string SUMMARY { get; set; }
        public string FIX_DATE { get; set; }
        public string CLOSE_DATE { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string INVOICE_NO { get; set; }
        public string JOBDESCRIPTION { get; set; }
        public string OWNER_ID { get; set; }
        public string TYPE_JOB { get; set; }
        public string SEQ { get; set; }
        public string SEQ_ENCRYPT { get; set; }
    }


}
