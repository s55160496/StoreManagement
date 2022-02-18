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
    public class AdjustPartController : BaseController
    {
        public AdjustPartController(IConfiguration config) : base(config)
        {

        }

        public IActionResult Index()
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {

                List<TBT_ADJ_SPAREPART> Model = new List<TBT_ADJ_SPAREPART>();

                Model = GET_TBT_ADJ_SPAREPART(out code);

                var SPAREPART = GET_TBM_SPAREPART(out code, new TBM_SPAREPART() { });
                ViewData["SPAREPART"] = SPAREPART?.ToArray();

                return View((Model != null ? Model.ToArray() : new TBT_ADJ_SPAREPART[] { }));
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

        public IActionResult ViewDetailAdjustPart(string str)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                List<TBT_ADJ_SPAREPART> Model = new List<TBT_ADJ_SPAREPART>();
                string ID = string.Empty;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    ID = STCrypt.Decrypt(str);
                }
                Model = GET_TBT_ADJ_SPAREPART_DETAIL(out code, ID);

                return View((Model != null ? Model.ToArray() : new TBT_ADJ_SPAREPART[] { }));
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
        public IActionResult SaveData(TBT_ADJ_SPAREPART data)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                if (SessionUserInfoIsExpired())
                {
                    code = HttpStatusCode.Unauthorized;
                    throw new Exception("Session time out");
                }

                data.CREATE_BY = SessionUserInfo().USER_ID;

                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBT_ADJ_SPAREPART", Method.POST);

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
