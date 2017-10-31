using Microsoft.AspNetCore.Mvc;

namespace Bob.Libraries.Extensions.MonitorWeb.Controllers
{
    public class WebSitesController : Controller
    {
        public WebSitesController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Edit(WebSiteModel model)
        //{
        //    return View();
        //}


    }

    
}