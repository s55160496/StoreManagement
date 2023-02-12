using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using StoreManagement.App_Extension;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using static StoreManagement.App_Extension.SysFunctions;

namespace StoreManagement.Controllers
{
    [ValidateSession]

    public class EmployeeController : BaseController
    {
        public EmployeeController(IConfiguration config) : base(config)
        {

        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult CreateEmployee(string str)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                string sUser_ID = string.Empty;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    sUser_ID = STCrypt.Decrypt(str);
                }

                TBM_EMPLOYEE model = new TBM_EMPLOYEE();

                var lstData = GET_TBM_EMPLOYEE(out code,new TBM_EMPLOYEE() { });
                string EDIT_FLG = "N";
                if (lstData != null && !string.IsNullOrEmpty(sUser_ID))
                {
                    model = lstData.Where(w => w.USER_ID == sUser_ID).FirstOrDefault();
                    EDIT_FLG = "Y";
                }

                ViewData["EDIT_FLG"] = EDIT_FLG;

                var POSITION = GET_EMPLOYEE_POSITION(out code);
                ViewData["POSITION"] = POSITION?.ToArray();

                var STORE = GET_TBM_LOCATION_STORE(out code,new TBM_LOCATION_STORE());
                ViewData["STORE"] = STORE?.ToArray();
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
        public IActionResult SaveData(TBM_EMPLOYEE data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                data.CREATE_BY = "1";

                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBM_EMPLOYEE", Method.POST);
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
        public IActionResult GetData(TBM_EMPLOYEE data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var lstData = GET_TBM_EMPLOYEE(out code,new TBM_EMPLOYEE() { });
                if (lstData != null)
                {
                    foreach (var item in lstData)
                    {
                        item.USER_ID_ENCRYPT = Encrypt_UrlEncrypt(item.USER_ID);
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
        public IActionResult DeleteData(TMN_DATA data)
        {
            data.USER_ID = "1";
            foreach (var item in data.DATA)
            {
                item.ID = SysFunctions.Decrypt_UrlDecode(item.ID);
            }
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("TERMINATE_TBM_EMPLOYEE", Method.POST);
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
        public IActionResult RPTData(TBM_EMPLOYEE data)
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
                var Data = RPT_TBM_EMPLOYEE(out code, data);
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
