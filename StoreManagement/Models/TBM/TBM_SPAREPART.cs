using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class TBM_SPAREPART
    {
        public string PART_ID { get; set; }
        public string PART_ID_ENCRYPT { get; set; }
        public string PART_NO { get; set; }
        public string PART_NAME { get; set; }
        public string PART_DESC { get; set; }
        public string PART_TYPE { get; set; }
        public string PART_TYPE_DESC { get; set; }
        public string COST_PRICE { get; set; }
        public string SALE_PRICE { get; set; }
        public string UNIT_CODE { get; set; }
        public string UNIT_NAME { get; set; }
        public string PART_VALUE { get; set; }
        public string PART_WEIGHT { get; set; }
        public string MINIMUM_VALUE { get; set; }
        public string MAXIMUM_VALUE { get; set; }

        public string LOCATION_ID { get; set; }
        public string LOCATION_NAME { get; set; }
        public string CREATE_DATE { get; set; }
        public string CREATE_BY { get; set; }
        public string CANCAL_DATE { get; set; }
        public string CANCEL_BY { get; set; }
        public string CANCEL_REASON { get; set; }
        public string UPDATE_DATE { get; set; }
        public string UPDATE_BY { get; set; }
        public string PAGE { get; set; }
        public string JOBID { get; set; }
        public string REF_OTHER { get; set; }
        public string REF_GROUP { get; set; }
        public string PATH_IMAGE { get; set; }

    }
}
