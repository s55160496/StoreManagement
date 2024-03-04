using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace StoreManagement.Controllers
{
    public class SaleContractJobController : BaseController
    {
        private string SaleContractJob = "http://203.151.136.81/SaleContractJob/Home/Link";
        public SaleContractJobController(IConfiguration config) : base(config)
        {
        }
        public IActionResult Index()
        {

            return Redirect($"{SaleContractJob}?token={SessionUserInfo().TOKEN}");
        }
    }
}
