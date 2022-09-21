using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Controllers
{
    [ValidateSession]

    public class ReportPPMController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ReportPPMController(IConfiguration config, IWebHostEnvironment hostingEnvironment) : base(config)
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
