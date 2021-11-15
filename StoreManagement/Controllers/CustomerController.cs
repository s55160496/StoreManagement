using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(IConfiguration config) : base(config)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateCustomer()
        {
            var PROVINCE = GET_PROVINCE();
            ViewData["PROVINCE"] = PROVINCE.ToArray();


            return View();
        }
    }
}
