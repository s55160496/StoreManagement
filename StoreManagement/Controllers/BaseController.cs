using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public JsonResult VerifyUserLogin(TM_User data)
        {
            try
            {
                VerifyUser(data);
                return Json(new { IsSuccess = false, data = "" });
            }
            catch (Exception ex)
            {

                return Json(new { IsSuccess = false, ErrorMsg = ex.Message });
            }
        }

        public void VerifyUser(TM_User data)
        {
            if (string.IsNullOrWhiteSpace(data.Username) || string.IsNullOrWhiteSpace(data.Password))
            {
                throw new Exception("Username or Password is null");
            }

            data.nUserID = 1;
            data.Name = "Phongsawat";
            data.SurName = "Pipatpholchailkul";
            data.Position = "Administrator";
            data.FullName = data.Name + " " + data.SurName;

            HttpContext.Session.Remove(UserAccount);
            HttpContext.Session.SetObjectAsJson(UserAccount, data);

        }

        public TM_User SessionUserInfo()
        {
            return HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount);
        }

        public bool SessionUserInfoIsExpired()
        {
            return HttpContext.Session.GetObjectFromJson<TM_User>(UserAccount) == null;
        }

        public List<PROVINCE> GET_PROVINCE()
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_PROVINCE", Method.GET);
                return client.Execute<List<PROVINCE>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<DISTRICT> GET_DISTRICT(string PROVINCE_CODE)
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_DISTRICT/" + PROVINCE_CODE, Method.GET);
                return client.Execute<List<DISTRICT>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SUB_DISTRICT> GET_SUB_DISTRICT(string DISTRICT_CODE)
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_SUB_DISTRICT/" + DISTRICT_CODE, Method.GET);
                return client.Execute<List<SUB_DISTRICT>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EMPLOYEE_POSITION> GET_EMPLOYEE_POSITION()
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_EMPLOYEE_POSITION", Method.GET);
                return client.Execute<List<EMPLOYEE_POSITION>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<JOBTYPE> GET_JOBTYPE()
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_JOBTYPE", Method.GET);
                return client.Execute<List<JOBTYPE>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<UNIT> GET_UNIT()
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_UNIT", Method.GET);
                return client.Execute<List<UNIT>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_EMPLOYEE> GET_TBM_EMPLOYEE(TBM_EMPLOYEE req)
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_EMPLOYEE", Method.POST);
                request.AddJsonBody(req);
                return client.Execute<List<TBM_EMPLOYEE>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<TBM_VEHICLE> GET_TBM_VEHICLE(TBM_VEHICLE req)
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_VEHICLE", Method.POST);
                request.AddJsonBody(req);
                return client.Execute<List<TBM_VEHICLE>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_CUSTOMER> GET_TBM_CUSTOMER(TBM_CUSTOMER req)
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_CUSTOMER", Method.POST);
                request.AddJsonBody(req);
                return client.Execute<List<TBM_CUSTOMER>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_CUSTOMER> GET_CUSTOMER_BY_JOB(string license_no)
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_CUSTOMER_BY_JOB?license_no=" + license_no, Method.POST);
                return client.Execute<List<TBM_CUSTOMER>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_LOCATION_STORE> GET_TBM_LOCATION_STORE(TBM_LOCATION_STORE req)
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_LOCATION_STORE", Method.POST);
                request.AddJsonBody(req);
                return client.Execute<List<TBM_LOCATION_STORE>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_SERVICES> GET_TBM_SERVICES(TBM_SERVICES req)
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_SERVICES", Method.POST);
                request.AddJsonBody(req);
                return client.Execute<List<TBM_SERVICES>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TBM_SPAREPART> GET_TBM_SPAREPART(TBM_SPAREPART req)
        {
            try
            {
                var client = new RestClient(URL_API);
                var request = new RestRequest("GET_TBM_SPAREPART", Method.POST);
                request.AddJsonBody(req);
                return client.Execute<List<TBM_SPAREPART>>(request).Data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
