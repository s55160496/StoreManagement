using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class CLOSEJOB
    {
        public string JOB_ID { get; set; }
        public string TYPE_JOB { get; set; }
        public string LICENSE_NO { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string SUMMARY { get; set; }
        public string ACTION { get; set; }
        public string RESULT { get; set; }
        public string EMAIL_CUSTOMER { get; set; }
        public string TRANSFER_TO { get; set; }
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
        public string FLG_CLOSE { get; set; } // Y : close , N : draft
    }
    public class job_file /*: DataFile*/
    {
        //public job_file(string path, string file_name)
        //{
        //    DataFile s = new DataFile(path);
        //    this.FileName = (string.IsNullOrEmpty(file_name) ? s.FileName : file_name);
        //    this.FileData = s.FileData;
        //    this.ContentType = s.ContentType;
        //}
        public string IMAGE_TYPE { get; set; }
        public string FileName
        { get; set; }

        public string ContentType
        { get; set; }

        public string FileData
        { get; set; }

    }
    public class tbt_job_checklist
    {

        public string CK_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public string CHECK_LIST { get; set; }
    }

    public class tbt_job_detail
    {
        public string BJOB_ID { get; set; }
        public string B1_MODEL { get; set; }
        public string B1_SERIAL { get; set; }
        public string B1_AMP_HRS { get; set; }
        public string B1_DATE_CODE { get; set; }
        public string B1_SPEC_GRAVITY { get; set; }
        public string B1_VOLT_STATIC { get; set; }
        public string B1_VOLT_LOAD { get; set; }
        public string B2_MODEL { get; set; }
        public string B2_SERIAL { get; set; }
        public string B2_AMP_HRS { get; set; }
        public string B2_DATE_CODE { get; set; }
        public string B2_SPEC_GRAVITY { get; set; }
        public string B2_VOLT_STATIC { get; set; }
        public string B2_VOLT_LOAD { get; set; }
        public string CD_MANUFACT { get; set; }
        public string CD_MODEL { get; set; }
        public string CD_SERIAL { get; set; }
        public string CD_TAG_DATE { get; set; }
        public string H_METER { get; set; }
        public string V_SERVICE_MANE { get; set; }
        public string V_LABOUR { get; set; }
        public string V_TRAVEL { get; set; }
        public string V_TOTAL { get; set; }
        public string FAILURE_CODE { get; set; }
        public string FAIR_WEAR { get; set; }
    }

    public class tbt_job_image
    {
        public string IJOB_ID { get; set; }
        public string SEQ { get; set; }
        public string IMG_NAME { get; set; }
        public string IMAGE_DESCRIPTION { get; set; }
        public string IMG_PATH { get; set; }
        public string IMAGE_TYPE { get; set; }
    }
    public class tbt_job_part
    {
        //public string pjob_id{get;set;}
        public string SEQ { get; set; }
        public string PART_NO { get; set; }
        public string PART_NAME { get; set; }
        public string PART_DESC { get; set; }
        public string PART_TYPE { get; set; }
        public string PART_TYPE_DESC { get; set; }

        public string TOTAL { get; set; }
        public string CREATE_DATE { get; set; }
        public string CREATE_BY { get; set; }
        public string STATUS { get; set; }
    }
}
