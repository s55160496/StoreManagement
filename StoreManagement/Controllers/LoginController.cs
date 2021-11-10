using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
