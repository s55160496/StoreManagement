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
    public class JobController : BaseController
    {
        public JobController(IConfiguration config) : base(config)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateJob()
        {
            var JOBTYPE = GET_JOBTYPE();
            ViewData["JOBTYPE"] = JOBTYPE.ToArray();

            var EMPLOYEE = GET_TBM_EMPLOYEE(new TBM_EMPLOYEE() { });
            ViewData["EMPLOYEE"] = EMPLOYEE.ToArray();

            return View();
        }

        [HttpPost]
        public IActionResult GET_CUSTOMER_BY_LICENSE_NO(string License_No)
        {
            try
            {
                var lstData = GET_CUSTOMER_BY_JOB(License_No);
                return Json(lstData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public IActionResult SaveData(CREATEJOB data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                data.USER_ID = "1";

                var client = new RestClient(URL_API);
                var request = new RestRequest("CREATEJOB", Method.POST);

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
        public IActionResult GetData(string USER_ID)
        {
            try
            {
                var lstData = GET_JOBDETAIL_LIST(USER_ID);
                if (lstData != null)
                {
                    foreach (var item in lstData)
                    {
                        item.JOB_ID_ENCRYPT = Encrypt_UrlEncrypt(item.JOB_ID);
                    }
                }
                return Json(lstData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IActionResult ModifyJob(string str)
        {
            try
            {
                MODIFYFY_JOB Model = new MODIFYFY_JOB();

                if (string.IsNullOrEmpty(str))
                {
                    throw new Exception("ไม่พบ JOB_ID");
                }

                str = SysFunctions.Decrypt_UrlDecode(str);

                var JOBDETAIL = GET_JOBDETAIL(str);
                Model.JOBDETAIL = JOBDETAIL;

                var CHECKLIST = GET_CHECKLIST();
                Model.CHECKLIST = CHECKLIST;

                var JOBTYPE = GET_JOBTYPE();
                ViewData["JOBTYPE"] = JOBTYPE.ToArray();

                var EMPLOYEE = GET_TBM_EMPLOYEE(new TBM_EMPLOYEE() { });
                ViewData["EMPLOYEE"] = EMPLOYEE.ToArray();

                var SPAREPART = GET_TBM_SPAREPART(new TBM_SPAREPART() { });
                ViewData["SPAREPART"] = SPAREPART.ToArray();

                ViewBag.UserID = "1";

                return View(Model);
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult CreateJobParts(string str)
        {

            return View();
        }
    }
}
