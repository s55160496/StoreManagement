using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Controllers
{
    public class StockController : BaseController
    {
        public StockController(IConfiguration config) : base(config)
        {

        }
        public IActionResult Index()
        {
            var a = GET_PROVINCE();
            VerifyUserLogin(new TM_User() { Username = "admin", Password = "1234" });

            return View();
        }
    }
}
