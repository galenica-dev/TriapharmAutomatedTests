using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataGenerator;

namespace WebApiClient.Controllers
{
    public class DataGenController : Controller
    {
        // GET: DataGen
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Avs()
        {
            AHVAVSNumberGenerator generatorAvs = new AHVAVSNumberGenerator();
            string ahvAvsNumber = generatorAvs.GenerateAHVAVSNumber();

            ViewBag.Message = "AHV/AVS number: " + ahvAvsNumber;

            return View();
        }

        // AvsJson
        [HttpGet]
        public JsonResult AvsJson()
        {
            AHVAVSNumberGenerator generatorAvs = new AHVAVSNumberGenerator();
            string ahvAvsNumber = generatorAvs.GenerateAHVAVSNumber();

            // Create the JSON response
            var response = new
            {
                AvsNumber = ahvAvsNumber
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }



        public ActionResult InsuranceCardNumber()
        {
            InsuranceCardNumberGenerator generatorICN = new InsuranceCardNumberGenerator();
            string cardNumber = generatorICN.GenerateCardNumber();

            ViewBag.Message = "InsuranceCardNumber: " + cardNumber;

            return View();
        }

        // InsuranceCardNumberJson
        [HttpGet]
        public JsonResult InsuranceCardNumberJson()
        {
            InsuranceCardNumberGenerator generatorICN = new InsuranceCardNumberGenerator();
            string cardNumber = generatorICN.GenerateCardNumber();

            // Create a JSON response with the generated card number
            var response = new
            {
                InsuranceCardNumber = cardNumber
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}