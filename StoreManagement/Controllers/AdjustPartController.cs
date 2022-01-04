using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                List<TBT_ADJ_SPAREPART> Model = new List<TBT_ADJ_SPAREPART>();
                Model = GET_TBT_ADJ_SPAREPART();
                return View(Model);
            }
            catch (Exception ex)
            {

                return RedirectToAction("_Error", "Home", new { msg = "Message :" + ex.Message + "</br>" + "StackTrace" + ex.StackTrace });

            }

        }
    }
}
