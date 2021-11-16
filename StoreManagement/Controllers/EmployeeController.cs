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

        public IActionResult CreateEmployee(string str)
        {
            TBM_EMPLOYEE model = new TBM_EMPLOYEE();

            var POSITION = GET_EMPLOYEE_POSITION();
            ViewData["POSITION"] = POSITION.ToArray();

            return View(model);
        }

        [HttpPost]
        public IActionResult SaveData(TBM_EMPLOYEE data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                data.CREATE_BY = "000000";

                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBM_EMPLOYEE", Method.POST);

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
        public IActionResult GetData(TBM_EMPLOYEE data)
        {
            try
            {
                var lstData = GET_EMPLOYEE(new TBM_EMPLOYEE() { });
                if (lstData.Any())
                {
                    foreach (var item in lstData)
                    {
                        item.USER_ID_ENCRYPT = Encrypt_UrlEncrypt(item.USER_ID);
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
