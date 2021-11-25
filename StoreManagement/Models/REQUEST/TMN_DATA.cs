using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class TMN_DATA
    {
        public string USER_ID { get; set; }
        public DATA_TMN[] DATA { get; set; }
    }

    public class DATA_TMN
    {
        public string ID { get; set; }
        public string  MSG { get; set; }
    }
}
