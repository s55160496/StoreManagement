using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class TBM_CUSTOMER
    {
        public string CUSTOMER_ID { get; set; }
        public string ID_CARD { get; set; }
        public string CUST_TYPE { get; set; }
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        public string ADDRESS { get; set; }
        public string SUB_DISTRICT_NO { get; set; }
        public string DISTRICT_CODE { get; set; }
        public string PROVINCE_CODE { get; set; }
        public string ZIP_CODE { get; set; }
        public string PHONE_NO { get; set; }
        public string EMAIL { get; set; }
    }
}
