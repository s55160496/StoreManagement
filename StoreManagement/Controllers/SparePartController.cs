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
    public class SparePartController : BaseController
    {
        public SparePartController(IConfiguration config) : base(config)
        {

        }
        public IActionResult Index()
        {
            var user = HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount);
            if (user.POSITION =="MN"|| user.POSITION == "OS")
            {
                ViewBag.noadd = "1";
            }
            else
            {
                ViewBag.noadd = "0";
            }
            
            return View();
        }

        public IActionResult CreateSparePart(string str)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                #region EDIT
                string ID = string.Empty;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    ID = STCrypt.Decrypt(str);
                }

                TBM_SPAREPART model = new TBM_SPAREPART();

                string EDIT_FLG = "N";
                var lstData = GET_TBM_SPAREPART(out code,new TBM_SPAREPART() { PART_ID = ID });
                if (lstData != null && !string.IsNullOrEmpty(ID))
                {
                    model = lstData.Where(w => w.PART_ID == ID).FirstOrDefault();
                    EDIT_FLG = "Y";
                }

                ViewData["EDIT_FLG"] = EDIT_FLG;
                #endregion

                var LOCATION = GET_TBM_LOCATION_STORE(out code, new TBM_LOCATION_STORE() { });
                ViewData["LOCATION"] = LOCATION?.ToArray();

                var UNIT = GET_UNIT(out code);
                ViewData["UNIT"] = UNIT?.ToArray();

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
        public IActionResult SaveData(TBM_SPAREPART data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                data.CREATE_BY = "1";
                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBM_SPAREPART", Method.POST);
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
        public IActionResult GetData(TBM_SPAREPART data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                //var lstData = GET_TBM_SPAREPART(out code,new TBM_SPAREPART() { });
                var lstData = GET_TBM_SPAREPART(out code, data);
                if (lstData != null && lstData.Any())
                {
                    foreach (var item in lstData)
                    {
                        item.PART_ID_ENCRYPT = Encrypt_UrlEncrypt(item.PART_ID);
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
    }
}
