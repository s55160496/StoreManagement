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
    public class CustomerController : BaseController
    {
        public CustomerController(IConfiguration config) : base(config)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateCustomer(string str)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                string ID = string.Empty;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    ID = STCrypt.Decrypt(str);
                }

                TBM_CUSTOMER model = new TBM_CUSTOMER();

                string EDIT_FLG = "N";
                var lstData = GET_TBM_CUSTOMER(out code,new TBM_CUSTOMER() { });
                if (lstData != null && !string.IsNullOrEmpty(ID))
                {
                    model = lstData.Where(w => w.CUSTOMER_ID == ID).FirstOrDefault();
                    EDIT_FLG = "Y";
                }

                ViewData["EDIT_FLG"] = EDIT_FLG;

                var PROVINCE = GET_PROVINCE(out code);
                ViewData["PROVINCE"] = PROVINCE?.ToArray();

                return View(model);
            }
            catch (Exception ex)
            {
                if(code == HttpStatusCode.Unauthorized)
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
        public IActionResult SaveData(TBM_CUSTOMER data)
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

                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBM_CUSTOMER", Method.POST);
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
        public IActionResult GetData(TBM_CUSTOMER data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var lstData = GET_TBM_CUSTOMER(out code,new TBM_CUSTOMER() { });
                if (lstData != null && lstData.Any())
                {
                    foreach (var item in lstData)
                    {
                        item.CUSTOMER_ID_ENCRYPT = Encrypt_UrlEncrypt(item.CUSTOMER_ID);
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
        public IActionResult GET_DISTRICT_BY_PROVINCE_CODE(string province_code)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var lstData = GET_DISTRICT(out code, province_code);
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
            //try
            //{
            //    var lstData = GET_DISTRICT(province_code);
            //    return Json(lstData);
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
        }

        [HttpPost]
        public IActionResult GET_SUB_DISTRICT_BY_DISTRICT_CODE(string district_code)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var lstData = GET_SUB_DISTRICT(out code, district_code);
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
            //try
            //{
            //    var lstData = GET_SUB_DISTRICT(district_code);
            //    return Json(lstData);
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
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
                if (SessionUserInfoIsExpired())
                {
                    code = HttpStatusCode.Unauthorized;
                    throw new Exception("Session time out");
                }

                var client = new RestClient(URL_API);
                var request = new RestRequest("TERMINATE_TBM_CUSTOMER", Method.POST);
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
    }
}
