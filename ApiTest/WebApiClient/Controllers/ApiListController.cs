using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApiClient.Controllers
{
    public class ApiListController : Controller
    {
        // GET: ApiList
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ServerConfig()
        {
            return View();
        }
        public ActionResult CustomerPersona() 
        {
            return View();
        }
        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Partners()
        {
            return View();
        }

        public ActionResult SalesOrder()
        {
            return View();
        }
    }
}