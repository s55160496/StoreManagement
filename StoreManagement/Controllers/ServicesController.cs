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
    public class ServicesController : BaseController
    {
        public ServicesController(IConfiguration config) : base(config)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateServices(string str)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                string ID = string.Empty;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    ID = STCrypt.Decrypt(str);
                }

                TBM_SERVICES model = new TBM_SERVICES();

                string EDIT_FLG = "N";
                var lstData = GET_TBM_SERVICES(out code,new TBM_SERVICES() { SERVICES_NO = ID });
                if (lstData != null && !string.IsNullOrEmpty(ID))
                {
                    model = lstData.Where(w => w.SERVICES_NO == ID).FirstOrDefault();
                    EDIT_FLG = "Y";
                }

                ViewData["EDIT_FLG"] = EDIT_FLG;

                var JOBTYPE = GET_JOBTYPE(out code);
                ViewData["JOBTYPE"] = JOBTYPE.ToArray();

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
        public IActionResult SaveData(TBM_SERVICES data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                data.CREATE_BY = "1";
                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBM_SERVICES", Method.POST);
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
        public IActionResult GetData(TBM_SERVICES data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var lstData = GET_TBM_SERVICES(out code,new TBM_SERVICES() { });
                if (lstData != null && lstData.Any())
                {
                    foreach (var item in lstData)
                    {
                        item.SERVICES_NO_ENCRYPT = Encrypt_UrlEncrypt(item.SERVICES_NO);
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
                var request = new RestRequest("TERMINATE_TBM_SERVICES", Method.POST);
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
    }
}
