using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace WebApiClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            ViewBag.AssemblyVersion = version;
            return View();
        }

    }
}