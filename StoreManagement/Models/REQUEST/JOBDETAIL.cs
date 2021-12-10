using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class JOBDETAIL
    {
        public string JOB_ID { get; set; }
        public string LICENSE_NO { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string SUMMARY { get; set; }
        public string ACTION { get; set; }
        public string RESULT { get; set; }
        public string TRANSFER_TO { get; set; }
        public string EMAIL_CUSTOMER { get; set; }
        public string INVOICE_NO { get; set; }
        public string OWNER_ID { get; set; }
        public string OWNER_NAME { get; set; }
        public string REF_HJOB_ID { get; set; }
        public string USERID { get; set; }
        public tbt_job_detail JOB_DETAIL { get; set; }
        public List<tbt_job_checklist> JOB_CHECKLISTS { get; set; }
        public List<job_file> JOB_IMAGES { get; set; }
        public List<tbt_job_image> IMAGE_DETAIL { get; set; }
        public List<tbt_job_part> JOB_PARTS { get; set; }
        public string FLG_CLOSE { get; set; }
    }
}
