using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using StoreManagement.App_Extension;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static StoreManagement.App_Extension.SysFunctions;

namespace StoreManagement.Controllers
{
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
            string ID = string.Empty;
            if (!string.IsNullOrWhiteSpace(str))
            {
                ID = STCrypt.Decrypt(str);
            }

            TBM_VEHICLE model = new TBM_VEHICLE();

            string EDIT_FLG = "N";
            var lstData = GET_TBM_VEHICLE(new TBM_VEHICLE() { });
            if (lstData != null && !string.IsNullOrEmpty(ID))
            {
                model = lstData.Where(w => w.LICENSE_NO == ID).FirstOrDefault();
                EDIT_FLG = "Y";
            }

            ViewData["EDIT_FLG"] = EDIT_FLG;

            var CUSTOMER = GET_TBM_CUSTOMER(new TBM_CUSTOMER() { });
            ViewData["CUSTOMER"] = CUSTOMER == null ? null : CUSTOMER.ToArray();

            return View(model);
        }

        [HttpPost]
        public IActionResult SaveData(TBM_VEHICLE data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBM_VEHICLE", Method.POST);

                request.AddJsonBody(data);

                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    var content = response.Content;

                }
                else
                {
                    throw new Exception(response.Content);
                }
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
                result.Status = SysFunctions.process_Failed;
            }

            return Json(result);
        }

        [HttpPost]
        public IActionResult GetData(TBM_VEHICLE data)
        {
            try
            {
                var lstData = GET_TBM_VEHICLE(new TBM_VEHICLE() { });
                if (lstData != null && lstData.Any())
                {
                    foreach (var item in lstData)
                    {
                        item.LICENSE_NO_ENCRYPT = Encrypt_UrlEncrypt(item.LICENSE_NO);
                    }
                }
                return Json(lstData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
