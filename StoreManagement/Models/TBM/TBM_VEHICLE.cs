using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class TBM_VEHICLE
    {
        public string LICENSE_NO { get; set; }
        public string LICENSE_NO_ENCRYPT { get; set; }
        public string SEQ { get; set; }
        public string BRAND_NO { get; set; }
        public string BRAND_NAME_THA { get; set; }
        public string MODEL_NO { get; set; }
        public string CHASSIS_NO { get; set; }
        public string COLOR { get; set; }
        public string EFFECTIVE_DATE { get; set; }
        public string EXPIRE_DATE { get; set; }
        public string SERVICE_PRICE { get; set; }
        public string SERVICE_NO { get; set; }
        public string CONTRACT_NO { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CONTRACT_TYPE { get; set; }
        public string CONTRACT_TYPE_NAME { get; set; }
        public string STD_PMP { get; set; }
        public string EMPLOYEE_ID { get; set; }
    }
}
