using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class REPORT_STOCK
    {
        public string PARTNO { get; set; }
        public string PARTID { get; set; }
        public string LOCATIONID { get; set; }
        public string PART_CREATE_FROM { get; set; }
        public string PART_CREATE_TO { get; set; }
        public string USERNAME { get; set; }
        public string REPORT_TYPE { get; set; }
    }

}
