﻿using Microsoft.AspNetCore.Mvc;
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
            try
            {
                string sUser_ID = string.Empty;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    sUser_ID = STCrypt.Decrypt(str);
                }

                TBM_EMPLOYEE model = new TBM_EMPLOYEE();

                var lstData = GET_TBM_EMPLOYEE(new TBM_EMPLOYEE() { });
                string EDIT_FLG = "N";
                if (lstData != null && !string.IsNullOrEmpty(sUser_ID))
                {
                    model = lstData.Where(w => w.USER_ID == sUser_ID).FirstOrDefault();
                    EDIT_FLG = "Y";
                }

                ViewData["EDIT_FLG"] = EDIT_FLG;

                var POSITION = GET_EMPLOYEE_POSITION();
                ViewData["POSITION"] = POSITION.ToArray();

                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult SaveData(TBM_EMPLOYEE data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                data.CREATE_BY = "1";

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
                var lstData = GET_TBM_EMPLOYEE(new TBM_EMPLOYEE() { });
                if (lstData != null)
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

        [HttpPost]
        public IActionResult DeleteData(TMN_DATA data)
        {
            data.USER_ID = "1";
            foreach (var item in data.DATA)
            {
                item.ID = SysFunctions.Decrypt_UrlDecode(item.ID);
            }
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("TERMINATE_TBM_EMPLOYEE", Method.POST);

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

    }
}
