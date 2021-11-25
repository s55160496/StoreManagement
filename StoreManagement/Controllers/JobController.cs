using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Controllers
{
    public class JobController : BaseController
    {
        public JobController(IConfiguration config) : base(config)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateJob()
        {
            var JOBTYPE = GET_JOBTYPE();
            ViewData["JOBTYPE"] = JOBTYPE.ToArray();

            var EMPLOYEE = GET_TBM_EMPLOYEE(new TBM_EMPLOYEE() { });
            ViewData["EMPLOYEE"] = EMPLOYEE.ToArray();

            return View();
        }

        [HttpPost]
        public IActionResult GET_CUSTOMER_BY_LICENSE_NO(string License_No)
        {
            try
            {
                var lstData = GET_CUSTOMER_BY_JOB(License_No);
                return Json(lstData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
