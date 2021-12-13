using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class FileUpLoad
    {
        public int nID { get; set; }
        public string sPath { get; set; }
        public string sSystemFileName { get; set; }
        public string sFileName { get; set; }
        public bool IsDelete { get; set; }
        public bool IsNew { get; set; }
        public string sSize { get; set; }
    }
}
