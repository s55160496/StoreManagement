using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Controllers
{
    public class UploadFileController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public UploadFileController(IWebHostEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SaveToServer(string savetopath)
        {
            var data = new ItemData();
            //IHostingEnvironment env = new HostingEnvironment();
            //if (HttpContext.Request.Form.Files.Count > 0)
            //var xx = System.IO.Directory.GetDirectoryRoot();
            //var x = Path.GetPathRoot;
            var files = HttpContext.Request.Form.Files;
            //if (HttpContext.Request.Form.Files.Count > 0)
            data.dStartDate = DateTime.Now;
            data.lstProcess = new List<ProcessLog>();
            if (HttpContext.Request.Form.Files.Count > 0)
            {
                string filepath = "";
                filepath = savetopath;
                string sFileName = "";
                string sSysFileName = "";
                string sFileType = "";
                for (int i = 0; i < HttpContext.Request.Form.Files.Count; i++)
                {
                    var a = new ProcessLog();
                    a.nLine = 47;
                    a.dStartDate = DateTime.Now;
                    a.sProcessname = "Line 47 - 99";
                    var file = HttpContext.Request.Form.Files[i];
                    sFileName = file.FileName;
                    string[] arrfilename = (sFileName + "").Split('.');
                    sFileType = file.ContentType;
                    var b = new ProcessLog();
                    var c = new ProcessLog();
                    b.nLine = 56;
                    b.dStartDate = DateTime.Now;
                    b.sProcessname = "Line 56 - 74";
                    if (string.IsNullOrEmpty(sSysFileName))
                    {
                        for (int j = 0; j < (arrfilename.Length - 1); j++)
                        {
                            sSysFileName += arrfilename[j];
                        }

                        sSysFileName = DateTime.Now.ToString("ddMMyyyyHHmmssff") + "." + arrfilename[arrfilename.Length - 1];
                    }
                    else
                    {
                        sSysFileName = sSysFileName + "." + arrfilename[arrfilename.Length - 1];

                    }
                    b.dEndDate = DateTime.Now;
                    b.TotalTime = (b.dEndDate - b.dStartDate).TotalSeconds.ToString();
                    c.nLine = 75;
                    c.dStartDate = DateTime.Now;
                    c.sProcessname = "Line 75 - 108";
                    if (!System.IO.Directory.Exists(MapPath(filepath)))
                    {

                        System.IO.Directory.CreateDirectory(MapPath(filepath));
                    }

                    //if (!System.IO.Directory.Exists(MapPathBackUp(filepath)))
                    //{
                    //    System.IO.Directory.CreateDirectory(MapPathBackUp(filepath));
                    //}
                    if (System.IO.Directory.Exists(MapPath(filepath)))
                    {


                        using (var stream = new FileStream(MapPath("./" + filepath + sSysFileName), FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        //using (var stream = new FileStream(MapPathBackUp("./" + filepath + sSysFileName), FileMode.Create))
                        //{
                        //    file.CopyTo(stream);
                        //}
                        filepath = filepath.Replace("../", "");
                        data.nID = 0;
                        data.IsCompleted = true;
                        data.SaveToFileName = sSysFileName;
                        data.FileName = sFileName;
                        data.SaveToPath = filepath;
                        data.url = filepath + sSysFileName;
                        data.IsNewFile = true;
                        data.IsNewChoose = true;
                        data.IsDelete = false;
                        data.sFileType = sFileType;
                    }
                    else
                    {
                        data.IsCompleted = false;
                        data.sMsg = "Error: Cannot create directory !";
                    }
                    c.dEndDate = DateTime.Now;
                    c.TotalTime = (c.dEndDate - c.dStartDate).TotalSeconds.ToString();
                    a.dEndDate = DateTime.Now;
                    a.TotalTime = (a.dEndDate - a.dStartDate).TotalSeconds.ToString();
                    data.lstProcess.Add(a);
                    data.lstProcess.Add(b);

                }
            }
            data.dEndDate = DateTime.Now;
            data.TotalTime = (data.dEndDate - data.dStartDate).TotalSeconds.ToString();

            return Json(new { d = data });
        }
        //public async Task SaveAsAsync(this IFormFile formFile, string filePath)
        //{
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await formFile.CopyToAsync(stream);
        //    }
        //}

        //public void SaveAs(this IFormFile formFile, string filePath)
        //{
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        formFile.CopyTo(stream);
        //    }
        //}
        public string MapPath(string path)
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, path);
            return filePath;
        }
        public string MapPathBackUp(string path)
        {
            string sDateNow = DateTime.Now.ToString("ddMMyyyy");
            var filePath = Path.Combine(Directory.GetCurrentDirectory().Replace("Code\\HMC_Vender_Register", "BackUp").Replace("HMC_Vendor_Test", "BackUp").Replace("Vendor", "BackUp") + "\\" + sDateNow + "\\", path.Replace("/", "\\"));
            return filePath;
        }
        public class ItemData
        {
            public int nID { get; set; }
            public string SaveToFileName { get; set; }
            public string FileName { get; set; }
            public string SaveToPath { get; set; }
            public string sSize { get; set; }
            /// <summary>
            /// for open file
            /// </summary>
            public string url { get; set; }
            public bool IsNewFile { get; set; }
            public bool IsCompleted { get; set; }
            public bool IsNewChoose { get; set; }
            public string sMsg { get; set; }
            public bool IsDelete { get; set; }
            public string sFileType { get; set; }
            public int? nFileTypeID { get; set; }
            public string sOrderNo { get; set; }
            public DateTime dStartDate { get; set; }
            public DateTime dEndDate { get; set; }
            public string TotalTime { get; set; }
            public List<ProcessLog> lstProcess { get; set; }
        }
        public class ProcessLog
        {
            public int nLine { get; set; }
            public string sProcessname { get; set; }
            public DateTime dStartDate { get; set; }
            public DateTime dEndDate { get; set; }
            public string TotalTime { get; set; }
        }
    }
}
