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
using System.Text.Json;
using System.Threading.Tasks;
using static StoreManagement.App_Extension.SysFunctions;

namespace StoreManagement.Controllers
{
    public class EmployeeController : BaseController
    {
        public EmployeeController(IConfiguration config) : base(config)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateEmployee(string user_id)
        {
            var a = GET_EMPLOYEE_POSITION();
            return View();
        }

        [HttpPost]
        public JsonResult SaveData(TBM_EMPLOYEE req)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBM_EMPLOYEE", Method.POST);

                request.AddJsonBody(req);

                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    var content = response.Content; // {"message":" created."}

                }
                else
                {
                    throw new Exception(response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
                result.Status = SysFunctions.process_SaveFail;
            }

            //var json = JsonConvert.SerializeObject(result,
            //new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //});
            //result  = JsonConvert.DeserializeObject<CResutlWebMethod>(response.Content.ReadAsStringAsync().Result);

            return Json(result);
        }
    }
}
