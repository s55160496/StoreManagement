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
    [ValidateSession]

    public static class REPORT_TYPE
    {
        public const string SEARCH = "SEARCH";
        public const string EXCEL = "EXCEL";
        public const string PDF = "PDF";
    }
    public class ReportJobController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ReportJobController(IConfiguration config, IWebHostEnvironment hostingEnvironment) : base(config)
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {

                var CUSTOMER = GET_TBM_CUSTOMER(out code, new TBM_CUSTOMER() { });
                ViewData["CUSTOMER"] = CUSTOMER == null ? null : CUSTOMER.ToArray();

                var TEACHNICIAL = GET_TBM_EMPLOYEE(out code, new TBM_EMPLOYEE() { });
                if (TEACHNICIAL.Any())
                {
                    TEACHNICIAL = TEACHNICIAL.Where(w => w.POSITION == "MN").ToList();
                }
                ViewData["TEACHNICIAL"] = TEACHNICIAL?.ToArray();

                var JOBTYPE = GET_JOBTYPE(out code);
                ViewData["JOBTYPE"] = JOBTYPE?.ToArray();

                return View();
            }
            catch (Exception ex)
            {
                if (code == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    return RedirectToAction("_Error", "Home", new { msg = "Message :" + ex.Message + "</br>" + "StackTrace" + ex.StackTrace });
                }
            }
        }

        [HttpPost]
        public IActionResult GetData(REPORT_JOB data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                if (SessionUserInfoIsExpired())
                {
                    code = HttpStatusCode.Unauthorized;
                    throw new Exception("Session time out");
                }
                data.USER_PRINT = HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount).USER_ID;

                if (REPORT_TYPE.SEARCH == data.REPORT_TYPE)
                {
                    var Data = GET_SUMMARY_JOB_LIST(out code, data);
                    if (Data != null)
                    {
                        if (Data.SUMMARY_JOB_LIST != null && Data.SUMMARY_JOB_LIST.Any())
                        {
                            foreach (var item in Data.SUMMARY_JOB_LIST)
                            {
                                item.JOB_ID_ENCRYPT = Encrypt_UrlEncrypt(item.JOB_ID);
                                item.SEQ_ENCRYPT = Encrypt_UrlEncrypt(item.SEQ);
                            }
                        }
                        result.data = Data;
                    }
                }
                else
                {
                    var Data = GET_SUMMARY_JOB_LIST_FILE(out code, data);
                    string tempLeter = "Report" + Guid.NewGuid().ToString();
                    SessionExtensions.Put(HttpContext.Session, tempLeter, Data);
                    result.data = tempLeter;
                }
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

        [HttpPost]
        public IActionResult sp_getReportDownTime(REPORT_JOB data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                if (SessionUserInfoIsExpired())
                {
                    code = HttpStatusCode.Unauthorized;
                    throw new Exception("Session time out");
                }
                data.USER_PRINT = HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount).USER_ID;
                var Data = sp_getReportDownTime(out code, data);
                string tempLeter = "Report" + Guid.NewGuid().ToString();
                SessionExtensions.Put(HttpContext.Session, tempLeter, Data);
                result.data = tempLeter;
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

        public IActionResult PreviewFile(string SessionRpt)
        {
            try
            {
                DataFile data_file = SessionExtensions.Get<DataFile>(HttpContext.Session, SessionRpt);
                if (data_file == null)
                {
                    throw new Exception("ไม่พบข้อมูล ทำการค้นหาข้อมูลอีกครั้ง");
                }

                return File(data_file.FileData, data_file.ContentType);


            }
            catch (Exception ex)
            {
                return RedirectToAction("_Error", "Home", new { msg = "Message :" + ex.Message + "</br>" + "StackTrace" + ex.StackTrace });
            }
        }
    }
}
