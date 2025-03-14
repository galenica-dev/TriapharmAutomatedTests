using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileServiceTest;
using MobileServiceTest.MobileServiceReference;
using WebApiClient.Models;
using DataFromDb;
using System.Threading.Tasks;

namespace WebApiClient.Controllers
{
    public class LocalServiceController : Controller
    {

        // GET: LocalService
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Medicine(string medicineName = null)
        {
            MobileServiceTest.ProductService productService = new MobileServiceTest.ProductService();
            List<MobileServiceTest.Product> searchedProducts = new List<MobileServiceTest.Product>();

            // If a search term is provided, perform the search
            if (!string.IsNullOrWhiteSpace(medicineName))
            {
                searchedProducts = productService.SearchArticles(medicineName);
            }

            // Pass the search results and search term to the view using ViewBag
            ViewBag.SearchedProducts = searchedProducts;
            ViewBag.SearchTerm = medicineName;

            return View();
        }

        // MedicineJson
        [HttpGet]
        public JsonResult MedicineJson(string medicine)
        {
            MobileServiceTest.ProductService productService = new MobileServiceTest.ProductService();
            List<MobileServiceTest.Product> searchedProducts = new List<MobileServiceTest.Product>();

            // Perform search if a term is provided
            if (!string.IsNullOrWhiteSpace(medicine))
            {
                searchedProducts = productService.SearchArticles(medicine);
            }

            // Transform search results into an anonymous type with matching structure
            var productList = searchedProducts
                .Select(p => new
                {
                    EanCode = p.EanCode,
                    Description = p.Description,
                    PharmaCode = p.PharmaCode,
                    QuantityOnStock = p.QuantityOnStock,
                    InStock = p.InStock,
                    Price = p.Price
                })
                .ToList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }




        public ActionResult PharmacyInfo()
        {
            var pharmacyInfo = Helper.PharmacyInfo.GetPharmacyInfo();

            // Pass the list to the view as the model
            return View(pharmacyInfo);
        }

        [HttpGet]
        public JsonResult PharmacyInfoJson()
        {
            var pharmacyInfo = Helper.PharmacyInfo.GetPharmacyInfo();

            // Return the pharmacy info as JSON data
            return Json(pharmacyInfo, JsonRequestBehavior.AllowGet);
        }

    }
}