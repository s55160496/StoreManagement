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

    }
}
