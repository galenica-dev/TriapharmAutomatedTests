using Antlr.Runtime.Misc;
using DataFromDb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace WebApiClient.Controllers
{
    public class DbController : Controller
    {
        string connectionStringArizona = "Server=localhost;Database=Arizona;Integrated Security=true;MultipleActiveResultSets=True;";
        string connectionStringActivePosRead = "Server=localhost;Database=ActivePos_read;Integrated Security=true;";
        string connectionStringActivePosServer = "Server=localhost;Database=ActivePos_server;Integrated Security=true;";
        int topRow = 50;
        int minRow = 1;
        CustomerService customerService = new CustomerService();
        RepetitionService repetitionService = new RepetitionService();

        private DateTime? ConvertToDateTime(object input)
        {
            // If the input is already a DateTime, return it directly
            if (input is DateTime dateTime)
            {
                return dateTime;
            }

            // If the input is a string in the "/Date(...)/" format
            if (input is string sqlDate && !string.IsNullOrEmpty(sqlDate))
            {
                var match = Regex.Match(sqlDate, @"\d+");
                if (match.Success && long.TryParse(match.Value, out long epochMilliseconds))
                {
                    // Convert the epoch milliseconds to DateTime
                    return DateTimeOffset.FromUnixTimeMilliseconds(epochMilliseconds).UtcDateTime;
                }
            }

            // Return null for unsupported formats
            return null;
        }

        // GET: Db
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Institutions()
        {
            var institutions = Institution.GetInstitutions(connectionStringArizona, topRow);

            ViewBag.Institutions = institutions;

            return View();
        }

        // InstitutionsJson
        [HttpGet]
        public JsonResult InstitutionsJson()
        {
            var institutions = Institution.GetInstitutions(connectionStringArizona, topRow);

            // Transform into a list of anonymous objects with a similar structure
            var institutionList = institutions
                .Select(i => new { AdName = i.AdName, ContractNumber = i.ContractNumber, UGuid = i.UGuid })
                .ToList();

            return Json(institutionList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult LimaProducts()
        {
            var products = ProductService.GetLimaProducts(connectionStringArizona);
            ViewBag.Products = products;

            return View();
        }

        // LimaProductsJson
        [HttpGet]
        public JsonResult LimaProductsJson()
        {
            var products = ProductService.GetLimaProducts(connectionStringArizona);

            // Transform into a list of anonymous objects with similar structure
            var productList = products
                .Select(p => new { ItkKey = p.ItkKey, FpPubPrice = p.FpPubPrice, FpMmrPrice = p.FpMmrPrice })
                .ToList();

            return Json(productList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CustomersInInstitution()
        {
            // Fetch the list of customers in the institution
            List<Customer> customersInInstitution = customerService.GetCustomerInInstitution(connectionStringActivePosRead, topRow);

            //Transform in to a list of tuples, because I have an issue with Objects in the view
            List<Tuple<string, string>> customerList = customersInInstitution
                .Select(c => Tuple.Create(c.FirstName, c.LastName))
                .ToList();

            // Pass the list to the view as the model
            return View(customerList);
        }

        // CustomersInInstitutionJson
        [HttpGet]
        public JsonResult CustomersInInstitutionJson()
        {
            // Fetch the list of customers in the institution
            List<Customer> customersInInstitution = customerService.GetCustomerInInstitution(connectionStringActivePosRead, topRow);

            // Transform into a list of anonymous objects with similar structure
            var customerList = customersInInstitution
                .Select(c => new { FirstName = c.FirstName, LastName = c.LastName })
                .ToList();

            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomersCard()
        {
            List<Customer> customersCard = customerService.GetCustomerCard(connectionStringActivePosRead, topRow);

            //Transform in to a list of tuples, because I have an issue with Objects in the view
            List<Tuple<string, string>> customerList = customersCard
                .Select(c => Tuple.Create(c.FirstName, c.LastName))
                .ToList();

            // Pass the customer to the view as the model
            return View(customerList);
        }

        // CustomersCardJson
        [HttpGet]
        public JsonResult CustomersCardJson()
        {
            List<Customer> customersCard = customerService.GetCustomerCard(connectionStringActivePosRead, topRow);

            // Transform into a list of anonymous objects with similar structure
            var customerList = customersCard
                .Select(c => new { FirstName = c.FirstName, LastName = c.LastName })
                .ToList();

            return Json(customerList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CustomerEmployee()
        {
            List<Customer> customersEmployee = customerService.GetCustomerEmployee(connectionStringActivePosRead, topRow);

            //Transform in to a list of tuples, because I have an issue with Objects in the view
            List<Tuple<string, string>> customerList = customersEmployee
                .Select(c => Tuple.Create(c.FirstName, c.LastName))
                .ToList();

            // Pass the customer to the view as the model
            return View(customerList);
        }

        // CustomerEmployeeJson
        [HttpGet]
        public JsonResult CustomerEmployeeJson()
        {
            List<Customer> customersEmployee = customerService.GetCustomerEmployee(connectionStringActivePosRead, topRow);

            // Transform into a list of anonymous objects with similar structure
            var customerList = customersEmployee
                .Select(c => new { FirstName = c.FirstName, LastName = c.LastName })
                .ToList();

            return Json(customerList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CustomerWithAVH()
        {
            List<Customer> customersAvh = customerService.GetCustomerWithAVH(connectionStringActivePosRead, topRow);

            //Transform in to a list of tuples, because I have an issue with Objects in the view
            List<Tuple<string, string>> customerList = customersAvh
                .Select(c => Tuple.Create(c.FirstName, c.LastName))
                .ToList();

            // Pass the customer to the view as the model
            return View(customerList);
        }

        // CustomerWithAVHJson
        [HttpGet]
        public JsonResult CustomerWithAVHJson()
        {
            List<Customer> customersAvh = customerService.GetCustomerWithAVH(connectionStringActivePosRead, topRow);

            // Transform into a list of anonymous objects with similar structure
            var customerList = customersAvh
                .Select(c => new { FirstName = c.FirstName, LastName = c.LastName })
                .ToList();

            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        //CustomerWithValidInsurance
        public ActionResult CustomerWithValidInsurance()
        {
            List<Customer> customersInsurance = customerService.GetCustomerWithValidInsurance(connectionStringActivePosRead, topRow);

            //Transform in to a list of tuples, because I have an issue with Objects in the view
            List<Tuple<string, string, string, string>> customerList = customersInsurance
                .Select(c => Tuple.Create(c.FirstName, c.LastName, c.AddressId.ToString(), c.InsuranceName))
                .ToList();

            // Pass the customer to the view as the model
            return View(customerList);
        }

        // CustomerWithValidInsuranceJson
        [HttpGet]
        public JsonResult CustomerWithValidInsuranceJson()
        {
            List<Customer> customersInsurance = customerService.GetCustomerWithValidInsurance(connectionStringActivePosRead, topRow);

            // Transform into a list of anonymous objects to match the structure
            var customerList = customersInsurance
                .Select(c => new { FirstName = c.FirstName, LastName = c.LastName, c.AddressId, c.InsuranceName })
                .ToList();

            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        //CustomerWithValidRepetition
        public ActionResult CustomerWithValidRepetition()
        {
            List<Customer> customersWithRepetition = customerService.GetCustomerWithValidRepetition(connectionStringActivePosRead, topRow);

            //Transform in to a list of tuples, because I have an issue with Objects in the view
            List<Tuple<string, string, string>> customerList = customersWithRepetition
                .Select(c => Tuple.Create(c.FirstName, c.LastName, ConvertToDateTime(c.DateOfBirth)?.ToString("yyyy-MM-dd")))
                .ToList();

            // Pass the customer to the view as the model
            return View(customerList);
        }

        // CustomerWithValidRepetitionJson
        [HttpGet]
        public JsonResult CustomerWithValidRepetitionJson()
        {
            List<Customer> customersWithRepetition = customerService.GetCustomerWithValidRepetition(connectionStringActivePosRead, topRow);

            // Transform into a list of anonymous objects with similar structure
            var customerList = customersWithRepetition
                .Select(c => new {  c.FirstName, c.LastName, DateOfBirth = ConvertToDateTime(c.DateOfBirth)?.ToString("yyyy-MM-dd"),  c.AddressId, c.OmnichannelUUID, c.PatientId })
                .ToList();

            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetCustomerRepetitions(int topRow, string patientId)
        {
            // Call the GetCustomerRepetition method to fetch the data
            List<Repetition> repetitions = repetitionService.GetCustomerRepetition(connectionStringActivePosRead, topRow, patientId);

            var customerList = repetitions
                .Select(r => new { r.RepetitionNo, r.PatientId, r.PhysicianId, r.ParentSalesOrderNo, 
                    r.RepetitionType, LimitDate = ConvertToDateTime(r.LimitDate)?.ToString("yyyy-MM-dd"),
                    DefaultEndDateRepetition = ConvertToDateTime(r.DefaultEndDateRepetition)?.ToString("yyyy-MM-dd"),
                    r.TotalQuantity, r.RemainingQuantity, r.PrescriptionNo, r.SalesOrderId, r.SalesOrderDetailId, 
                    r.ItemDescription, r.ItemId,r.Error })
                .ToList();

            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        //SetCvLoginSecondsAcceptance
        [HttpGet]
        public JsonResult SetCvLoginSecondsAcceptance(string newValue)
        {
            // Define the key for the record you want to update
            string key = "cvLoginSecondsAcceptance";

            // Call the UpdateValue method to update the database and get the result object
            var result = CommonVar.UpdateValue(connectionStringArizona, key, newValue);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // GetCvLoginSecondsAcceptance
        [HttpGet]
        public JsonResult GetCvLoginSecondsAcceptance()
        {
            // Define the key for the record you want to retrieve
            string key = "cvLoginSecondsAcceptance";

            // Call the GetValue method to fetch the value from the database
            var result = CommonVar.GetValue(connectionStringArizona, key);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SetCvWebShopShippingPharmacy(string newValue)
        {
            // Define the key for the record you want to update
            string key = "cvWebShopShippingPharmacy";

            // Call the UpdateValue method to update the database and get the result object
            var result = CommonVar.UpdateValue(connectionStringArizona, key, newValue);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCvWebShopShippingPharmacy()
        {
            // Define the key for the record you want to retrieve
            string key = "cvWebShopShippingPharmacy";

            // Call the GetValue method to fetch the value from the database
            var result = CommonVar.GetValue(connectionStringArizona, key);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CvWebShopShippingPharmacy()
        {
            // Return the view for testing the APIs
            return View();
        }

        [HttpGet]
        public JsonResult GetSuppliersByName(string searchTerm)
        {
            try
            {
                var result = SupplierService.GetSuppliersByName(connectionStringArizona, searchTerm);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Supplier()
        {
            // Return the view for testing the APIs
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetProductsInfo(
            string searchTerm = null,
            bool isRequiringFridge = false,
            bool isNarcotic = false,
            bool isRequiringSubscription = false,
            bool hasMinMaxQty = false,
            Biosimilar biosimilar = Biosimilar.Empty,
            string gtinEanCode = null,
            string pharmacode = null,
            bool needGtinEanCode = false,
            Language language = Language.French,
            decimal? priceEqBigger = null,
            decimal? priceEqSmaller = null,
            int? quantityStockEqBigger = null,
            int? quantityStockEqSmaller = null,
            string supplierName = null,
            string supplierCode = null,
            int? itemId = null,
            int nbRow = 100)
        {
            var products = ProductService.GetProducts(
                connectionStringArizona,
                searchTerm,
                isRequiringFridge,
                isNarcotic,
                isRequiringSubscription,
                hasMinMaxQty,
                biosimilar,
                gtinEanCode,
                pharmacode,
                needGtinEanCode,
                language,
                priceEqBigger,
                priceEqSmaller,
                quantityStockEqBigger,
                quantityStockEqSmaller,
                supplierName,
                supplierCode,
                itemId,
                nbRow);

            return Json(products, JsonRequestBehavior.AllowGet);
        }


        // GET: ProductInfo
        public ActionResult ProductInfo()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetSalesOrder(
            string externalReferenceNumber = null,
            string userTrigram = null,
            string patientName = null,
            string paymentType = null,
            string onlineOrderId = null,
            int? deliveryMode = null,
            Guid? onlineOrderFulfillmentId = null,
            int? onlineSaleStatus = null,
            int? customerId = null,
            int nbRow = 100)
        {
            // Call the SalesOrderService.GetSalesOrder method
            List<SalesOrder> salesOrders = SalesOrderService.GetSalesOrder(
                connectionStringActivePosServer,
                externalReferenceNumber,
                userTrigram,
                patientName,
                paymentType,
                onlineOrderId,
                deliveryMode,
                onlineOrderFulfillmentId,
                onlineSaleStatus,
                customerId,
                nbRow
            );

            // Return the result as JSON
            return Json(salesOrders, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetProductsLore(
            string searchTerm = null,
            string gtinEanCode = null,
            string pharmacode = null,
            bool needGtinEanCode = false,
            Language language = Language.French,
            int nbRow = 100) 
        { 
            var products = ProductLoreService.GetProducts(
                connectionStringArizona,
                searchTerm,
                gtinEanCode,
                pharmacode,
                needGtinEanCode,
                language,
                nbRow);

            // Return the result as JSON
            return Json(products, JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult ProductsLore()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetProductLinks(
            string searchTerm = null,
            string gtinEanCode = null,
            string pharmacode = null,
            int linkType = 5, // Defaut replacement
            bool needGtinEanCode = false,
            Language language = Language.French,
            int nbRow = 100) 
        {
            var products = ProductLinksService.GetProducts(
                connectionStringArizona,
                searchTerm,
                gtinEanCode,
                pharmacode,
                linkType,
                needGtinEanCode,
                language,
                nbRow);

            // Return the result as JSON
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductLinks()
        {
            return View();
        }
    }
}