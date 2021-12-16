using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace StoreManagement.App_Extension
{
    public class SysFunctions
    {
        public static string process_SessionExpired { get { return "SSEXP"; } }
        public static string process_Success { get { return "Success"; } }
        public static string process_Failed { get { return "Failed"; } }
        public static string process_Duplicate { get { return "DUP"; } }
        public static string process_SaveFail { get { return "SaveFail"; } }

        public  class CResutlWebMethod
        {
            public int ID { get; set; }
            public string Status { get; set; }
            public string Msg { get; set; }
            public string Content { get; set; }
            public object data { get; set; }
        }

        public static int? GetIntNull(string sVal)
        {
            int? nTemp = null;
            int nCheck = 0;
            if (!string.IsNullOrEmpty(sVal))
            {
                sVal = ConvertExponentialToString(sVal);
                bool cCheck = int.TryParse(sVal, out nCheck);
                if (cCheck)
                {
                    nTemp = int.Parse(sVal);
                }
            }

            return nTemp;
        }

        public static int GetIntNullToZero(string sVal)
        {
            int nTemp = 0;
            int nCheck = 0;
            if (!string.IsNullOrEmpty(sVal))
            {
                sVal = ConvertExponentialToString(sVal);
                bool cCheck = int.TryParse(sVal, out nCheck);
                if (cCheck)
                {
                    nTemp = int.Parse(sVal);
                }
            }

            return nTemp;
        }

        public static decimal GetNumberNullToZero(string sVal)
        {
            decimal nTemp = 0;
            sVal = ConvertExponentialToString(sVal);
            nTemp = decimal.TryParse(sVal, out nTemp) ? nTemp : 0;
            return nTemp;
        }

        public static int ParseInt(string sVal)
        {
            int nTemp = 0;
            nTemp = int.TryParse(sVal, out nTemp) ? nTemp : 0;
            return nTemp;
        }

        public static decimal ParseDecimal(string sVal)
        {
            decimal nTemp = 0;
            nTemp = decimal.TryParse(sVal, out nTemp) ? nTemp : 0;
            return nTemp;
        }

        public static bool IsNumberic(string sVal)
        {
            decimal nTemp = 0;
            sVal = ConvertExponentialToString(sVal);
            return decimal.TryParse(sVal, out nTemp);
        }
        public static string ConvertExponentialToString(string sVal)
        {
            string sRsult = "";
            try
            {
                decimal nTemp = 0;
                bool check = Decimal.TryParse((sVal + "").Replace(",", ""), System.Globalization.NumberStyles.Float, null, out nTemp);
                if (check)
                {
                    decimal d = Decimal.Parse((sVal + "").Replace(",", ""), System.Globalization.NumberStyles.Float);
                    sRsult = (d + "").Replace(",", "");
                }
                else
                {
                    sRsult = sVal;
                }
            }
            catch
            {
                sRsult = sVal;
            }

            return sRsult != null ? (sRsult.Length < 50 ? sRsult : sRsult.Remove(50)) : ""; //เพื่อไม่ให้ตอน Save Error หากค่าที่เกิดจากผลการคำนวนเกิน Type ใน DB (varchar(50))
        }

        public static string ConvertDatetimeToString(DateTime? dDate)
        {
            string result = "";

            if (dDate.HasValue)
            {
                result = dDate.Value.Year + "-" + dDate.Value.Month.ToString().PadLeft(2, '0') + "-" + dDate.Value.Day.ToString().PadLeft(2, '0');
            }

            return result;
        }
        public static DateTime? ConvertStringToDateTime(string strDate)
        {
            string strTime = "";
            DateTime? dResult = null;
            DateTime dTemp;
            bool checkDate = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp);
            if (!checkDate)
            {
                if (strTime.Length < 5)
                {
                    dResult = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? "0" + strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
                }
            }
            else
            {
                dResult = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : (DateTime?)null;
            }

            return dResult;
        }

        public static string Encrypt_UrlEncrypt(string sValue)
        {
            string result = "";
            if (!string.IsNullOrEmpty(sValue))
            {
                result = System.Net.WebUtility.UrlEncode(STCrypt.Encrypt(sValue));
            }
            return result;
        }

        public static string Decrypt_UrlDecode(string sValue)
        {
            string result = "";
            if (!string.IsNullOrEmpty(sValue))
            {
                result = STCrypt.Decrypt(sValue);
            }
            return result;
        }
    }
}