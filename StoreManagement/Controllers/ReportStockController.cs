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
            try
            {
                var CUSTOMER = GET_TBM_CUSTOMER(out code, new TBM_CUSTOMER() { });
                ViewData["CUSTOMER"] = CUSTOMER == null ? null : CUSTOMER.ToArray();

                var EMPLOYEE = GET_TBM_EMPLOYEE(out code, new TBM_EMPLOYEE() { });
                if (EMPLOYEE.Any())
                {
                    EMPLOYEE = EMPLOYEE.Where(w => w.POSITION == "MN").ToList();
                }
                ViewData["EMPLOYEE"] = EMPLOYEE?.ToArray();

                var LOCATION = GET_TBM_LOCATION_STORE(out code, new TBM_LOCATION_STORE() { });
                ViewData["LOCATION"] = LOCATION?.ToArray();
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
        public IActionResult GetData(REPORT_STOCK data)
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
                data.USERNAME = HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount).USER_ID;

                if (REPORT_TYPE.SEARCH == data.REPORT_TYPE)
                {
                    var Data = GET_SUMMARY_STOCK_LIST(out code, data);
                    if (Data != null)
                    {
                        if (Data.SUMMARY_STOCK_LIST != null && Data.SUMMARY_STOCK_LIST.Any())
                        {
                            foreach (var item in Data.SUMMARY_STOCK_LIST)
                            {
                                item.PART_ID_ENCRYPT = Encrypt_UrlEncrypt(item.PART_ID);
                                item.PART_NO_ENCRYPT = Encrypt_UrlEncrypt(item.PART_NO);
                            }
                        }
                        result.data = Data;
                    }
                }
                else
                {
                    var Data = GET_SUMMARY_STOCK_LIST_FILE(out code, data);
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
        public IActionResult sp_get_movement_sparepart(REPORT_STOCK data)
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
                data.USERNAME = HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount).USER_ID;
                var Data = sp_get_movement_sparepart(out code, data);
                //string tempLeter = $"{DateTime.Now.ToString("yyyyMMddHHmm")}.xls";           
                
                return File(Convert.FromBase64String(Data.FileData),Data.ContentType , Data.FileName);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }

           
        }
    }
}
