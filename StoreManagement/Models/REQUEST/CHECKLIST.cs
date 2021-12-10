using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class CHECKLIST
    {
        public string CH_GROUP_ID { get; set; }
        public string CHECK_GROUP_NAME { get; set; }
        public string STATUS { get; set; }
        public string CREATE_DATE { get; set; }
        public string CREATE_BY { get; set; }
        public string UPDATE_DATE { get; set; }
        public string UPDATE_BY { get; set; }
        public List<CHECK_LIST> CHECK_LIST { get; set; }
    }

    public class CHECK_LIST
    {
        public string CH_ID { get; set; }
        public string CHECK_NAME { get; set; }
        public string STATUS { get; set; }
        public string CHECK_GROUP_ID { get; set; }
        public string CREATE_DATE { get; set; }
        public string CREATE_BY { get; set; }
        public string UPDATE_DATE { get; set; }
        public string UPDATE_BY { get; set; }
    }
}
