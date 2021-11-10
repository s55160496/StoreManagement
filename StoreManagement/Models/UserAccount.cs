using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreManagement
{
    /// <summary>
    /// Summary description for UserAccount
    /// </summary>
    public class UserAccount
    {
        public int nUserID { get; set; }
        public string sEmpCode { get; set; }
        public string sLogon_Nam { get; set; }
        public string sEmpFullName { get; set; }
        public string sEmpName { get; set; }
        public string sEmpSurName { get; set; }
        public string sEmpEmail { get; set; }

        private static string sSessionName = "UserAccount";
        private static UserAccount SS_UserAccount
        {
            get
            {
                var ssUA = HttpContext.Session.GetObjectFromJson<UserAccount>(sSessionName);
                return ssUA is UserAccount && ssUA != null ? (UserAccount)ssUA : null;
            }
            set { HttpContext.Session.GetObjectFromJson<UserAccount>(sSessionName) = value; }
        }

        public static void Login(UserAccount ua) { SS_UserAccount = ua; }
        public static void Login(TM_User u)
        {
            UserAccount ua = new UserAccount();
            ua.nUserID = u.nUserID;
            Login(ua);
        }

        public static void Logout() { SS_UserAccount = null; }

        public static UserAccount SessionInfo { get { return SS_UserAccount; } }
        public static bool IsExpired { get { return SS_UserAccount == null; } }


    }
}