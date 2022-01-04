using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class TBT_ADJ_SPAREPART
    {
        public string ADJ_ID { get; set; }
        public string PART_ID { get; set; }
        public string PART_NO { get; set; }
        public string ADJ_PART_VALUE { get; set; }
        public string CREATE_DATE { get; set; }
        public string CREATE_BY { get; set; }
        public string UPDATE_DATE { get; set; }
        public string UPDATE_BY { get; set; }
        public string CANCEL_DATE { get; set; }
        public string CANCEL_BY { get; set; }
        public string CANCEL_REASON { get; set; }
        public string PART_NAME { get; set; }
        public string PART_DESC { get; set; }
    }
}
