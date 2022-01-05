using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class TM_User
    {
        public string token { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string lastname { get; set; }
        public string position { get; set; }
        public string position_description { get; set; }
        public List<tbm_menu> menu { get; set; }
    }
    public class tbm_menu
    {
        public string menu_id { get; set; }
        public string menu_description { get; set; }
        public string status { get; set; }
        public string icon { get; set; }
        public string menu_controller { get; set; }
    }
}