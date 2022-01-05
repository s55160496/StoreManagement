﻿using Microsoft.AspNetCore.Http;
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
    public class BaseController : Controller
    {
        private IConfiguration _configuration;

        public static string process_SessionExpired = "SSEXP";
        public static string process_Success = "Success";
        public static string process_Failed = "Failed";
        public static string process_FileOversize = "OverSize";
        public static string process_FileInvalidType = "InvalidType";
        public static string process_Duplicate = "DUP";
        public static string process_SaveFail = "SaveFail";

        protected readonly string UserAccount = "UserAccount";
        protected readonly string URL_API = null;
        public BaseController(IConfiguration _configuration)
        {
            URL_API = _configuration.GetValue<string>("URL_API");
        }

        public JsonResult VerifyUserLogin(LOGIN data)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                VerifyUser(out code,data);
                //return Json(new { IsSuccess = false, data = "" });
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

        public void VerifyUser(out HttpStatusCode code, LOGIN data)
        {
            if (string.IsNullOrWhiteSpace(data.USERNAME) || string.IsNullOrWhiteSpace(data.PASSWORD))
            {
                throw new Exception("Username or Password is null");
            }
            code = HttpStatusCode.OK;
            var dataLogin = LOGIN(out code, data);
            //data.nUserID = 1;
            //data.Name = "Phongsawat";
            //data.SurName = "Pipatpholchailkul";
            //data.Position = "Administrator";
            //data.FullName = data.Name + " " + data.SurName;

            HttpContext.Session.Remove(UserAccount);
            HttpContext.Session.SetObjectAsJson(UserAccount, dataLogin);

        }

        public TM_User SessionUserInfo()
        {
            return HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount);
        }

        public bool SessionUserInfoIsExpired()
        {
            return HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount) == null;
        }

        public TM_User LOGIN(out HttpStatusCode code, LOGIN req)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("Login", Method.POST);
                request.AddJsonBody(req);
                var response = client.Execute<TM_User>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<PROVINCE> GET_PROVINCE(out HttpStatusCode code)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_PROVINCE", Method.GET);
                var response = client.Execute<List<PROVINCE>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<DISTRICT> GET_DISTRICT(out HttpStatusCode code, string PROVINCE_CODE)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_DISTRICT/" + PROVINCE_CODE, Method.GET);
                var response = client.Execute<List<DISTRICT>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SUB_DISTRICT> GET_SUB_DISTRICT(out HttpStatusCode code, string DISTRICT_CODE)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_SUB_DISTRICT/" + DISTRICT_CODE, Method.GET);
                var response = client.Execute<List<SUB_DISTRICT>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EMPLOYEE_POSITION> GET_EMPLOYEE_POSITION(out HttpStatusCode code)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_EMPLOYEE_POSITION", Method.GET);
                var response = client.Execute<List<EMPLOYEE_POSITION>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<JOBTYPE> GET_JOBTYPE(out HttpStatusCode code)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_JOBTYPE", Method.GET);
                var response = client.Execute<List<JOBTYPE>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<UNIT> GET_UNIT(out HttpStatusCode code)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_UNIT", Method.GET);
                var response = client.Execute<List<UNIT>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_EMPLOYEE> GET_TBM_EMPLOYEE(out HttpStatusCode code, TBM_EMPLOYEE req)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_EMPLOYEE", Method.POST);
                request.AddJsonBody(req);
                var response = client.Execute<List<TBM_EMPLOYEE>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<TBM_VEHICLE> GET_TBM_VEHICLE(out HttpStatusCode code, TBM_VEHICLE req)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_VEHICLE", Method.POST);
                request.AddJsonBody(req);
                var response = client.Execute<List<TBM_VEHICLE>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_CUSTOMER> GET_TBM_CUSTOMER(out HttpStatusCode code, TBM_CUSTOMER req)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_CUSTOMER", Method.POST);
                request.AddJsonBody(req);
                var response = client.Execute<List<TBM_CUSTOMER>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_CUSTOMER> GET_CUSTOMER_BY_JOB(out HttpStatusCode code, string license_no)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_CUSTOMER_BY_JOB?license_no=" + license_no, Method.POST);
                var response = client.Execute<List<TBM_CUSTOMER>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_LOCATION_STORE> GET_TBM_LOCATION_STORE(out HttpStatusCode code, TBM_LOCATION_STORE req)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_LOCATION_STORE", Method.POST);
                request.AddJsonBody(req);
                var response = client.Execute<List<TBM_LOCATION_STORE>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_SERVICES> GET_TBM_SERVICES(out HttpStatusCode code, TBM_SERVICES req)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_SERVICES", Method.POST);
                request.AddJsonBody(req);
                var response = client.Execute<List<TBM_SERVICES>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_SPAREPART> GET_TBM_SPAREPART(out HttpStatusCode code, TBM_SPAREPART req)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_SPAREPART", Method.POST);
                request.AddJsonBody(req);
                var response = client.Execute<List<TBM_SPAREPART>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_BRAND> GET_TBM_BRAND(out HttpStatusCode code)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_BRAND", Method.GET);
                var response = client.Execute<List<TBM_BRAND>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region JOB
        public List<JOBDETAIL_LIST> GET_JOBDETAIL_LIST(out HttpStatusCode code, string user_id)
        {

            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_JOBDETAIL_LIST/" + user_id, Method.GET);
                var response = client.Execute<List<JOBDETAIL_LIST>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public CLOSEJOB GET_CLOSE_JOB_DETAIL(out HttpStatusCode code, string job_id)
        {

            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_JOBDETAIL?job_id=" + job_id, Method.POST);
                var response = client.Execute<CLOSEJOB>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CHECKLIST> GET_CHECKLIST(out HttpStatusCode code)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("CHECKLIST", Method.GET);
                var response = client.Execute<List<CHECKLIST>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        public string TERMINATE_TBT_JOB_IMAGE(out HttpStatusCode code, string ijob_id, string seq)
        {
            try
            {
                code = HttpStatusCode.OK;
                string r = string.Empty;
                var client = new RestClient(URL_API);
                var request = new RestRequest("TERMINATE_TBT_JOB_IMAGE?ijob_id=" + ijob_id + "&seq=" + seq, Method.POST);
                var response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    return response.Content;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public job_file GET_FILE(out HttpStatusCode code, string ijob_id, string seq)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_FILE/" + ijob_id + "/" + seq, Method.GET);
                var response = client.Execute<job_file>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TBT_ADJ_SPAREPART> GET_TBT_ADJ_SPAREPART(out HttpStatusCode code)
        {
            try
            {
                code = HttpStatusCode.OK;
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBT_ADJ_SPAREPART", Method.GET);
                var response = client.Execute<List<TBT_ADJ_SPAREPART>>(request);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
                else
                {
                    code = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new Exception(response.StatusDescription);
                    }
                    else
                    {
                        throw new Exception(response.ErrorMessage);
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public string INSERT_TBT_ADJ_SPAREPART(out HttpStatusCode code,TBT_ADJ_SPAREPART req)
        //{
        //    try
        //    {
        //code = HttpStatusCode.OK;
        //        var client = new RestClient(URL_API);
        //        var request = new RestRequest("INSERT_TBT_ADJ_SPAREPART", Method.POST);
        //        request.AddJsonBody(req);

        //        IRestResponse response = client.Execute(request);
        //                if (response.IsSuccessful)
        //                {
        //                    return response.Data;
        //                }
        //                else
        //                {
        //                    code = response.StatusCode;
        //                    if (response.StatusCode == HttpStatusCode.Unauthorized)
        //                    {
        //                        throw new Exception(response.StatusDescription);
        //}
        //                    else
        //{
        //    throw new Exception(response.ErrorMessage);
        //}

        //                }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
    }
}
