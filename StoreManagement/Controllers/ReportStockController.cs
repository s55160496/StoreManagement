using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StoreManagement.Controllers
{
    public class ReportStockController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ReportStockController(IConfiguration config, IWebHostEnvironment hostingEnvironment) : base(config)
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            HttpStatusCode code = HttpStatusCode.OK;
            var CUSTOMER = GET_TBM_CUSTOMER(out code, new TBM_CUSTOMER() { });
            ViewData["CUSTOMER"] = CUSTOMER == null ? null : CUSTOMER.ToArray();

            var EMPLOYEE = GET_TBM_EMPLOYEE(out code, new TBM_EMPLOYEE() { });
            ViewData["EMPLOYEE"] = EMPLOYEE.ToArray();

            var LOCATION = GET_TBM_LOCATION_STORE(out code, new TBM_LOCATION_STORE() { });
            ViewData["LOCATION"] = LOCATION.ToArray();

            return View();
        }
    }
}
