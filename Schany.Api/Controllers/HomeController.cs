using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schany.Core.Dto;
using StackExchangeRedis;

namespace Schany.Api.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Appsetting的读取方式
        /// </summary>
        public AppsettingDataModel appsetting;
        public HomeController(IOptions<AppsettingDataModel> option)
        {
            appsetting = option.Value;
        }

        public IActionResult Index()
        {            
            return View();
        }
    }
}