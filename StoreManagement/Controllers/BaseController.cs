using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Controllers
{
    public class BaseController : Controller
    {
        protected readonly string UserAccount = "UserAccount";
        public BaseController(IConfiguration config)
        {

        }

        public JsonResult VerifyUserLogin(TM_User data)
        {
            try
            {
                VerifyUser(data);
                return Json(new { IsSuccess = false, data = "" });
            }
            catch (Exception ex)
            {

                return Json(new { IsSuccess = false, ErrorMsg = ex.Message });
            }
        }

        public void VerifyUser(TM_User data)
        {
            if (string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Password))
            {
                throw new Exception("Username or Password is null");
            }

            data.nUserID = 1;
            data.Name = "Phongsawat";
            data.SurName = "Pipatpholchailkul";
            data.Position = "Administrator";
            data.FullName = data.Name + " " + data.SurName;

            HttpContext.Session.Remove(UserAccount);
            HttpContext.Session.SetObjectAsJson(UserAccount, data);

        }

        public TM_User SessionUserInfo()
        {
            return HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount);
        }

        public bool SessionUserInfoIsExpired()
        {
            return HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount) == null;
        }
    }
}
