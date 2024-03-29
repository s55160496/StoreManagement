﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class TBM_EMPLOYEE
    {
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string PASSWORD { get; set; }
        public string FULLNAME { get; set; }
        public string LASTNAME { get; set; }
        public string IDCARD { get; set; }
        public string POSITION { get; set; }
        public string POSITION_DESCRIPTION { get; set; }
        public string STATUS { get; set; }
        public string CREATE_BY { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_DATE { get; set; }
        public string UPDATE_BY { get; set; }
        public string USER_ID_ENCRYPT { get; set; }
        public string SHOWSTOCK { get; set; }
        public string LOCATION_NAME { get; set; }
    }
}
