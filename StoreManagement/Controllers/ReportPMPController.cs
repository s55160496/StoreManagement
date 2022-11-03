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
    [ValidateSession]

    public class ReportPMPController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ReportPMPController(IConfiguration config, IWebHostEnvironment hostingEnvironment) : base(config)
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {

            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                if (SessionUserInfoIsExpired())
                {
                    code = HttpStatusCode.Unauthorized;
                    throw new Exception("Session time out");
                }
                var result = sp_getReportPPM(out code);

                return View(result);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        [HttpGet]
        public IActionResult ExportExcel()
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var file = sp_getReportPPM(out code, "EXCEL");
                return File(Convert.FromBase64String(file.FileData), file.ContentType, file.FileName);
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
                return View("Error");
            }

        }
        [HttpGet]
        public IActionResult ExportPdf()
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var file = sp_getReportPPM(out code, "PDF");
                return File(Convert.FromBase64String(file.FileData), file.ContentType, file.FileName);
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
                return View("Error");
            }

        }
    }
}
