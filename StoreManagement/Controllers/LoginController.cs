using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using StoreManagement.App_Extension;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static StoreManagement.App_Extension.SysFunctions;

namespace StoreManagement.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(IConfiguration config) : base(config)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(LOGIN data)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                VerifyUser(out code, data);
              //  VerifyUserLogin(new LOGIN() { USERNAME = data.USERNAME, PASSWORD = data.PASSWORD });
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
                if (code == HttpStatusCode.Unauthorized)
                {
                    result.Status = SysFunctions.process_SessionExpired;
                }
                else if(code == HttpStatusCode.BadRequest)
                {
                    result.Status = SysFunctions.process_Failed;
                }
                else
                {
                    result.Status = SysFunctions.process_Failed;
                }
            }

            return Json(result);
        }

        [HttpGet]
        public IActionResult Token(string token)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                code = HttpStatusCode.OK;
                var dataLogin = GetUserLogin(out code, token);

                //data.nUserID = 1;
                //data.Name = "Phongsawat";
                //data.SurName = "Pipatpholchailkul";
                //data.Position = "Administrator";
                //data.FullName = data.Name + " " + data.SurName;

                HttpContext.Session.Remove(UserAccount);
                HttpContext.Session.SetObjectAsJson(UserAccount, dataLogin);
                //  VerifyUserLogin(new LOGIN() { USERNAME = data.USERNAME, PASSWORD = data.PASSWORD });
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
                if (code == HttpStatusCode.Unauthorized)
                {
                    result.Status = SysFunctions.process_SessionExpired;
                }
                else if (code == HttpStatusCode.BadRequest)
                {
                    result.Status = SysFunctions.process_Failed;
                }
                else
                {
                    result.Status = SysFunctions.process_Failed;
                }
            }

            return Json(result);
        }

    }
}
