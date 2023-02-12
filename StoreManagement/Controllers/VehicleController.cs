using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
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

    public class VehicleController : BaseController
    {
        public VehicleController(IConfiguration config) : base(config)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateVehicle(string str)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                string ID = string.Empty;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    ID = STCrypt.Decrypt(str);
                }

                TBM_VEHICLE model = new TBM_VEHICLE();

                string EDIT_FLG = "N";
                var lstData = GET_TBM_VEHICLE(out code,new TBM_VEHICLE() { });
                if (lstData != null && !string.IsNullOrEmpty(ID))
                {
                    model = lstData.Where(w => w.LICENSE_NO == ID).FirstOrDefault();
                    EDIT_FLG = "Y";
                }

                ViewData["EDIT_FLG"] = EDIT_FLG;

                var CUSTOMER = GET_TBM_CUSTOMER(out code, new TBM_CUSTOMER() { });
                ViewData["CUSTOMER"] = CUSTOMER == null ? null : CUSTOMER.ToArray();

                var SERVICES = GET_TBM_SERVICES(out code, new TBM_SERVICES() { });
                ViewData["SERVICES"] = SERVICES == null ? null : SERVICES.ToArray();

                var BRAND = GET_TBM_BRAND(out code);
                ViewData["BRAND"] = BRAND == null ? null : BRAND.ToArray();

                var CONTRACT = GET_CONTRACT_TYPE(out code);
                ViewData["CONTRACT"] = CONTRACT == null ? null : CONTRACT.ToArray();

                var EMPLOYEE = GET_EMPLOYEE(out code);
                ViewData["EMPLOYEE"] = EMPLOYEE == null ? null : EMPLOYEE.ToArray();

                return View(model);
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
        public IActionResult SaveData(TBM_VEHICLE data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBM_VEHICLE", Method.POST);
                if (SessionUserInfoIsExpired())
                {
                    code = HttpStatusCode.Unauthorized;
                    throw new Exception("Session time out");
                }
                request.AddHeader("Authorization", "Bearer " + SessionUserInfo().TOKEN);
                request.AddJsonBody(data);

                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    var content = response.Content;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        throw new Exception(response.Content);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }
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
        public IActionResult GetData(TBM_VEHICLE data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var lstData = GET_TBM_VEHICLE(out code,new TBM_VEHICLE() { });
                if (lstData != null && lstData.Any())
                {
                    foreach (var item in lstData)
                    {
                        item.LICENSE_NO_ENCRYPT = Encrypt_UrlEncrypt(item.LICENSE_NO);
                    }
                }
                result.data = lstData;
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
        public IActionResult RPTData(TBM_VEHICLE data)
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
               // data.USER_PRINT = HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount).USER_ID;
                var Data = RPT_TBM_VEHICLE(out code, data);
                string tempLeter = "Report" + Guid.NewGuid().ToString("n");
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

        public IActionResult Export(string SessionRpt)
        {
            try
            {
                DataFile data_file = SessionExtensions.Get<DataFile>(HttpContext.Session, SessionRpt);
                if (data_file == null)
                {
                    throw new Exception("ไม่พบข้อมูล ทำการค้นหาข้อมูลอีกครั้ง");
                }
                if (data_file.ContentType == "application/vnd.ms-excel")
                    return File(data_file.FileData, data_file.ContentType, data_file.FileName);
                return File(data_file.FileData, data_file.ContentType);


            }
            catch (Exception ex)
            {
                return RedirectToAction("_Error", "Home", new { msg = "Message :" + ex.Message + "</br>" + "StackTrace" + ex.StackTrace });
            }
        }
    }
}
