using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class TBM_LOCATION_STORE
    {
        public string LOCATION_ID { get; set; }
        public string LOCATION_NAME { get; set; }
        public string OWNER_ID { get; set; }
        public string OWNER_NAME { get; set; }
        public string CREATE_BY { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_BY { get; set; }
        public string UPDATE_DATE { get; set; }
        public string STATUS { get; set; }
    }
}
