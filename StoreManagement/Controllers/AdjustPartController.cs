using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StoreManagement.Controllers
{
    public class AdjustPartController : BaseController
    {
        public AdjustPartController(IConfiguration config) : base(config)
        {

        }

        public IActionResult Index()
        {
            HttpStatusCode code = HttpStatusCode.OK;
            try
            {
                List<TBT_ADJ_SPAREPART> Model = new List<TBT_ADJ_SPAREPART>();
                
                Model = GET_TBT_ADJ_SPAREPART(out code);
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
    }
}
