using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class ReportJobController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ReportJobController(IConfiguration config, IWebHostEnvironment hostingEnvironment) : base(config)
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetData(REPORT_JOB data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var Data = GET_SUMMARY_JOB_LIST(out code, data);
                if (Data != null)
                {
                    if (Data.SUMMARY_JOB_LIST.Any())
                    {
                        foreach (var item in Data.SUMMARY_JOB_LIST)
                        {
                            item.JOB_ID_ENCRYPT = Encrypt_UrlEncrypt(item.JOB_ID);
                        }
                    }
                }
                result.data = Data;
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
                if (code == HttpStatusCode.Unauthorized)
                {
                    result.Status = SysFunctions.process_SessionExpired;
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
