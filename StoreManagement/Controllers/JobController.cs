using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using StoreManagement.App_Extension;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static StoreManagement.App_Extension.SysFunctions;
using static StoreManagement.Controllers.UploadFileController;

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
        public IActionResult CloseJob(CLOSEJOB data, FileUpLoad[] arr_file)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                data.USERID = "1";
                if (!string.IsNullOrEmpty(data.SIGNNATURE))
                {
                    string path = @"UploadFile\\Temp";
                    path = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\", path.Replace("/", "\\"));

                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }

                    string sSysFileName = DateTime.Now.ToString("ddMMyyyyHHmmssff");
                    string imageName = sSysFileName + ".png";

                    //set the image path
                    string imgPath = Path.Combine(path, imageName);

                    var arr = data.SIGNNATURE.Split(',');
                    byte[] imageBytes = Convert.FromBase64String(arr[1]);

                    System.IO.File.WriteAllBytes(imgPath, imageBytes);

                    if (data.JOB_IMAGES == null)
                    {
                        data.JOB_IMAGES = new List<job_file>();
                    }

                    path = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\", path.Replace("/", "\\"), imageName);

                    job_file jt = new job_file(path, "");
                    jt.IMAGE_TYPE = "sig";
                    data.JOB_IMAGES.Add(jt);

                }

                if (arr_file != null && arr_file.Any())
                {
                    if (data.JOB_IMAGES == null)
                    {
                        data.JOB_IMAGES = new List<job_file>();
                    }

                    foreach (var item in arr_file)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\", item.sPath.Replace("/", "\\"), item.sSystemFileName);
                        job_file jt = new job_file(path, item.sFileName);
                        jt.IMAGE_TYPE = "pic";

                        data.JOB_IMAGES.Add(jt);
                    }

                }

                var client = new RestClient(URL_API);
                var request = new RestRequest("CLOSEJOB", Method.POST);

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

                if (string.IsNullOrEmpty(str))
                {
                    throw new Exception("ไม่พบ JOB_ID");
                }

                str = SysFunctions.Decrypt_UrlDecode(str);

                MODIFYFY_JOB Model = new MODIFYFY_JOB();
                Model.JOBDETAIL = new JOBDETAIL();
                Model.JOBDETAIL.JOB_ID = str;

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

        //public class FileData
        //{
        //    public int nID { get; set; }
        //    public string SaveToFileName { get; set; }
        //    public string FileName { get; set; }
        //    public string SaveToPath { get; set; }
        //    public string sSize { get; set; }
        //}
    }
}
