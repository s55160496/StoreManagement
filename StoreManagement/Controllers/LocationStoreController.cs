﻿using Microsoft.AspNetCore.Mvc;
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
    public class LocationStoreController : BaseController
    {
        public LocationStoreController(IConfiguration config) : base(config)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateLocationStore(string str)
        {
            #region Edit
            string ID = string.Empty;
            if (!string.IsNullOrWhiteSpace(str))
            {
                ID = STCrypt.Decrypt(str);
            }

            TBM_LOCATION_STORE model = new TBM_LOCATION_STORE();

            string EDIT_FLG = "N";
            var lstData = GET_TBM_LOCATION_STORE(new TBM_LOCATION_STORE() { LOCATION_ID = ID });
            if (lstData != null && !string.IsNullOrEmpty(ID))
            {
                model = lstData.Where(w => w.LOCATION_ID == ID).FirstOrDefault();
                EDIT_FLG = "Y";
            }

            ViewData["EDIT_FLG"] = EDIT_FLG;

            #endregion

            var EMPLOYEE = GET_TBM_EMPLOYEE(new TBM_EMPLOYEE() { });
            ViewData["EMPLOYEE"] = EMPLOYEE.ToArray();
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveData(TBM_LOCATION_STORE data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                data.CREATE_BY = "1";
                var client = new RestClient(URL_API);
                var request = new RestRequest("INSERT_TBM_LOCATION_STORE", Method.POST);

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
        public IActionResult GetData(TBM_LOCATION_STORE data)
        {
            try
            {
                var lstData = GET_TBM_LOCATION_STORE(new TBM_LOCATION_STORE() { });
                if (lstData != null && lstData.Any())
                {
                    foreach (var item in lstData)
                    {
                        item.LOCATION_ID_ENCRYPT = Encrypt_UrlEncrypt(item.LOCATION_ID);
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
                var request = new RestRequest("TERMINATE_TBM_LOCATION_STORE", Method.POST);

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