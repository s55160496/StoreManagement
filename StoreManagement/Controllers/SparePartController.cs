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
    public class SparePartController : BaseController
    {
        public SparePartController(IConfiguration config) : base(config)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateSparePart()
        {
            var LOCATION = GET_TBM_LOCATION_STORE(new TBM_LOCATION_STORE() { });
            ViewData["LOCATION"] = LOCATION.ToArray();

            var UNIT = GET_UNIT();
            ViewData["UNIT"] = UNIT.ToArray();

            return View();
        }

        [HttpPost]
        public IActionResult SaveData(TBM_SPAREPART data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                data.CREATE_BY = "1";
                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBM_SPAREPART", Method.POST);

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
        public IActionResult GetData(TBM_SPAREPART data)
        {
            try
            {
                var lstData = GET_TBM_SPAREPART(new TBM_SPAREPART() { });
                if (lstData.Any())
                {
                    foreach (var item in lstData)
                    {
                        item.PART_ID_ENCRYPT = Encrypt_UrlEncrypt(item.PART_ID);
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
