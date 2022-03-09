using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using StoreManagement.App_Extension;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static StoreManagement.App_Extension.SysFunctions;
using static StoreManagement.Controllers.UploadFileController;

namespace StoreManagement.Controllers
{
    public class JobController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public JobController(IConfiguration config, IWebHostEnvironment hostingEnvironment) : base(config)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateJob()
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var JOBTYPE = GET_JOBTYPE(out code);
                ViewData["JOBTYPE"] = JOBTYPE.ToArray();

                var EMPLOYEE = GET_TBM_EMPLOYEE(out code, new TBM_EMPLOYEE() { });
                ViewData["EMPLOYEE"] = EMPLOYEE.ToArray();

                return View();
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
        public IActionResult GET_CUSTOMER_BY_LICENSE_NO(string License_No)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var lstData = GET_CUSTOMER_BY_JOB(out code, License_No);
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

        [HttpPost]
        public IActionResult SaveData(CREATEJOB data)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                //data.USER_ID = "1";

                var client = new RestClient(URL_API);
                var request = new RestRequest("CREATEJOB", Method.POST);
                if (SessionUserInfoIsExpired())
                {
                    code = HttpStatusCode.Unauthorized;
                    throw new Exception("Session time out");
                }
                data.USER_ID = HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount).USER_ID;
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
        public IActionResult CloseJob(CLOSEJOB data, FileUpLoad[] arr_file, string SIGNNATURE)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                //data.USERID = "1";
                if (SessionUserInfoIsExpired())
                {
                    code = HttpStatusCode.Unauthorized;
                    throw new Exception("Session time out");
                }
                data.USERID = HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount).USER_ID;

                if (!string.IsNullOrEmpty(SIGNNATURE))
                {
                    string path = @"UploadFile\Temp\";
                    //path = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\", path.Replace("/", "\\"));
                    path = Path.Combine(_hostingEnvironment.WebRootPath, path);

                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }

                    string sSysFileName = DateTime.Now.ToString("ddMMyyyyHHmmssff");
                    string imageName = sSysFileName + ".png";

                    //set the image path
                    string imgPath = Path.Combine(path, imageName);

                    var arr = SIGNNATURE.Split(',');
                    byte[] imageBytes = Convert.FromBase64String(arr[1]);
                    bool Ispass = true;
                    if (data.JOB_STATUS == "C" || data.JOB_STATUS == "F")
                    {
                        if (imageBytes.Length == 3416)
                        {
                            Ispass = false;
                            throw new Exception("ระบุ ลายเช็นต์");
                        }
                    }
                    else
                    {
                        if (imageBytes.Length == 3416)
                        {
                            Ispass = false;
                        }
                    }

                    if (Ispass)
                    {
                        if (data.JOB_IMAGES == null)
                        {
                            data.JOB_IMAGES = new List<job_file>();
                        }

                        System.IO.File.WriteAllBytes(imgPath, imageBytes);
                        //path = Path.Combine(_hostingEnvironment.ContentRootPath + "\\wwwroot\\", path.Replace("/", "\\"), imageName);

                        DataFile df = new DataFile(imgPath);
                        job_file jt = new job_file();
                        jt.ContentType = df.ContentType;
                        jt.FileName = df.FileName;
                        jt.FileData = Convert.ToBase64String(df.FileData);
                        jt.IMAGE_TYPE = "sig";
                        data.JOB_IMAGES.Add(jt);
                    }
                }

                if (arr_file != null && arr_file.Any())
                {
                    if (data.JOB_IMAGES == null)
                    {
                        data.JOB_IMAGES = new List<job_file>();
                    }

                    foreach (var item in arr_file)
                    {
                        string path = Path.Combine(_hostingEnvironment.WebRootPath, item.sPath, item.sSystemFileName);
                        DataFile df = new DataFile(path);
                        job_file jt = new job_file();
                        jt.ContentType = df.ContentType;
                        jt.FileData = Convert.ToBase64String(df.FileData);
                        jt.FileName = item.sFileName;
                        jt.IMAGE_TYPE = "pic";

                        data.JOB_IMAGES.Add(jt);
                    }

                }

                if (data.JOB_STATUS == "C" || data.JOB_STATUS == "F")
                {
                    if (data.JOB_PARTS !=null && data.JOB_PARTS.Any())
                    {
                        foreach (var item in data.JOB_PARTS)
                        {
                            if (string.IsNullOrWhiteSpace(item.TOTAL))
                            {
                                throw new Exception("ระบุ QTY ให้ครบถ้วน");
                            }
                        }
                    }
                }

                var client = new RestClient(URL_API);
                var request = new RestRequest("CLOSEJOB", Method.POST);
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
                    result.Msg = response.Content;
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
        public IActionResult GetData(string USER_ID)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var lstData = GET_JOBDETAIL_LIST(out code, USER_ID);
                if (lstData != null)
                {
                    foreach (var item in lstData)
                    {
                        item.JOB_ID_ENCRYPT = Encrypt_UrlEncrypt(item.JOB_ID);
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

        [HttpPost]
        public IActionResult GET_CHECK_STOCK(string part_id, string location_id)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                var Data = CHECK_STOCK(out code, part_id, location_id);
                result.data = Data;
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

        public IActionResult ModifyJob(string str)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    throw new Exception("ไม่พบ JOB_ID");
                }

                str = Decrypt_UrlDecode(str);

                CLOSEJOB Model = new CLOSEJOB();

                var CLOSE_JOB_DETAIL = GET_CLOSE_JOB_DETAIL(out code, str);
                if (CLOSE_JOB_DETAIL != null)
                {
                    Model = CLOSE_JOB_DETAIL;
                    if (Model.JOB_DETAIL == null)
                    {
                        Model.JOB_DETAIL = new tbt_job_detail();
                    }

                    if (Model.IMAGE_DETAIL == null)
                    {
                        Model.IMAGE_DETAIL = new List<tbt_job_image>();
                    }
                }

                foreach (var item in Model.IMAGE_DETAIL)
                {
                    item.IJOB_ID_ENCRYPT = Encrypt_UrlEncrypt(item.IJOB_ID);
                    item.SEQ_ENCRYPT = Encrypt_UrlEncrypt(item.SEQ);
                }

                var CHECK_LIST_MASTER = GET_CHECKLIST(out code);
                ViewData["CHECK_LIST_MASTER"] = CHECK_LIST_MASTER.ToArray();

                var JOBTYPE = GET_JOBTYPE(out code);
                ViewData["JOBTYPE"] = JOBTYPE.ToArray();

                var EMPLOYEE = GET_TBM_EMPLOYEE(out code, new TBM_EMPLOYEE() { });
                ViewData["EMPLOYEE"] = EMPLOYEE.ToArray();

                var SPAREPART = GET_TBM_SPAREPART(out code, new TBM_SPAREPART() { PAGE = "JOB" });
                ViewData["SPAREPART"] = SPAREPART.ToArray();

                ViewBag.UserID = HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount).USER_ID;

                return View(Model);
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

        public IActionResult CreateJobParts(string str)
        {

            return View();
        }


        [HttpPost]
        public IActionResult SaveSignature(string SIGNATURE)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            FileUpLoad fu = new FileUpLoad();

            try
            {
                if (!string.IsNullOrEmpty(SIGNATURE))
                {
                    string path = @"UploadFile\Temp\";
                    path = Path.Combine(_hostingEnvironment.WebRootPath, path);

                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }

                    string sSysFileName = DateTime.Now.ToString("ddMMyyyyHHmmssff");
                    string imageName = sSysFileName + ".png";

                    //set the image path
                    string imgPath = Path.Combine(path, imageName);

                    var arr = SIGNATURE.Split(',');

                    byte[] imageBytes = Convert.FromBase64String(arr[1]);
                    if (imageBytes.Length == 3416)
                    {
                        throw new Exception("ระบุ ลายเช็นต์");
                    }

                    System.IO.File.WriteAllBytes(imgPath, imageBytes);
                    DataFile df = new DataFile(imgPath);

                    fu.IsDelete = false;
                    fu.IsNew = true;
                    fu.nID = 1;
                    fu.sFileName = sSysFileName;
                    fu.sPath = imgPath;

                    //job_file jt = new job_file();
                    //jt.ContentType = df.ContentType;
                    //jt.FileName = df.FileName;
                    //jt.FileData = Convert.ToBase64String(df.FileData);
                    //jt.IMAGE_TYPE = "sig";
                    //data.JOB_IMAGES.Add(jt);
                    result.data = Convert.ToBase64String(df.FileData);
                }
            }
            catch (Exception ex)
            {
                result.Msg = ex.Message;
                result.Status = SysFunctions.process_Failed;
            }

            return Json(result);
        }

        //public class FileData
        //{
        //    public int nID { get; set; }
        //    public string SaveToFileName { get; set; }
        //    public string FileName { get; set; }
        //    public string SaveToPath { get; set; }
        //    public string sSize { get; set; }
        //}

        public IActionResult ViewImage(string IJOB_ID, string SEQ)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                if (string.IsNullOrWhiteSpace(IJOB_ID) && string.IsNullOrWhiteSpace(SEQ))
                {
                    throw new Exception("IJOB_ID or SEQ is null");
                }

                IJOB_ID = Decrypt_UrlDecode(IJOB_ID);
                SEQ = Decrypt_UrlDecode(SEQ);

                var file = GET_FILE(out code, IJOB_ID, SEQ);
                if (file == null)
                {
                    throw new Exception("ไม่พบ ไฟล์");
                }

                return File(Convert.FromBase64String(file.FileData), file.ContentType);
                //return View(file);
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
        public IActionResult DeleteImageDetail(string IJOB_ID, string SEQ)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            CResutlWebMethod result = new CResutlWebMethod();
            try
            {
                if (string.IsNullOrWhiteSpace(IJOB_ID) && string.IsNullOrWhiteSpace(SEQ))
                {
                    throw new Exception("IJOB_ID or SEQ is null");
                }

                IJOB_ID = Decrypt_UrlDecode(IJOB_ID);
                SEQ = Decrypt_UrlDecode(SEQ);
                result.Msg = TERMINATE_TBT_JOB_IMAGE(out code, IJOB_ID, SEQ);
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
