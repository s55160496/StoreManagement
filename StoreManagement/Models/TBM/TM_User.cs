using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class TM_User
    {
        public string TOKEN { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string PASSWORD { get; set; }
        public string FULLNAME { get; set; }
        public string LASTNAME { get; set; }
        public string POSITION { get; set; }
        public string POSITION_DESCRIPTION { get; set; }
        public List<tbm_menu> MENU { get; set; }
    }
    public class tbm_menu
    {
        public string MENU_ID { get; set; }
        public string MENU_DESCRIPTION { get; set; }
        public string STATUS { get; set; }
        public string ICON { get; set; }
        public string MENU_CONTROLLER { get; set; }
    }
}