using DataFromDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eRxCHMED16Service;
using static eRxCHMED16Service.ApiClientConvertToChmed16A;
using System.Net.Http;
using System.Threading.Tasks;
using OrderHubApi;
using MobileServiceTest;
using System.Text.Json;
using WebApiClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApiClient.Helper;
using static OrderHubApi.CreateOrder;

namespace WebApiClient.Controllers
{
    public class RemoteServiceController : Controller
    {
        string connectionStringArizona = "Server=localhost;Database=Arizona;Integrated Security=true;";
        string connectionStringActivePosRead = "Server=localhost;Database=ActivePos_read;Integrated Security=true;";
        int topRow = 50;
        int minRow = 1;
        CustomerService customerService = new CustomerService();

        // GET: RemoteService
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> eRx(int? AddressId)
        {
            List<Customer> customers;
            if (AddressId.HasValue)
            {
                // Search by AddressId
                customers = customerService.GetCustomerByAddressId(connectionStringActivePosRead, minRow, AddressId.Value);
                ViewBag.SearchType = "Customer by AddressId";
            }
            else
            {
                // Default: Get customer from institution
                customers = customerService.GetCustomerInInstitution(connectionStringActivePosRead, minRow);
                ViewBag.SearchType = "Persona : 1 random customer in institution";
            }

            if (customers != null && customers.Count > 0)
            {
                Customer customer = customers[0];
                ViewBag.FirstName = customer.FirstName;
                ViewBag.LastName = customer.LastName;

                ApiClientConvertToChmed16A apiClientConvertToChmed16A = new ApiClientConvertToChmed16A();

                // HappyPath CatB
                HttpResponseMessage responseConvertToChmed16A = await apiClientConvertToChmed16A.SendPostRequest(customer, Scenario.HappyPath, MedicineCategory.B);
                string response = await responseConvertToChmed16A.Content.ReadAsStringAsync();
                ViewBag.HappyPathResponseB = response;

                // HappyPath CatA
                responseConvertToChmed16A = await apiClientConvertToChmed16A.SendPostRequest(customer, Scenario.HappyPath, MedicineCategory.A);
                response = await responseConvertToChmed16A.Content.ReadAsStringAsync();
                ViewBag.HappyPathResponseA = response;

                //HappyPath CatC
                responseConvertToChmed16A = await apiClientConvertToChmed16A.SendPostRequest(customer, Scenario.HappyPath, MedicineCategory.C);
                response = await responseConvertToChmed16A.Content.ReadAsStringAsync();
                ViewBag.HappyPathResponseC = response;

                // Date Expired CatA
                responseConvertToChmed16A = await apiClientConvertToChmed16A.SendPostRequest(customer, Scenario.DateExpired, MedicineCategory.A);
                response = await responseConvertToChmed16A.Content.ReadAsStringAsync();
                ViewBag.DateExpiredResponseA = response;
            }
            else
            {
                // Handle case when no customers are found
                ViewBag.FirstName = "Not found";
                ViewBag.LastName = "Not found";
                ViewBag.HappyPathResponseB = "No data available";
                ViewBag.HappyPathResponseA = "No data available";
                ViewBag.DateExpiredResponseA = "No data available";
            }

            return View();
        }


        [HttpGet]
        public async Task<JsonResult> eRxJson(int? AddressId)
        {
            List<Customer> customers;
            if (AddressId.HasValue)
            {
                // Retrieve customer by AddressId
                customers = customerService.GetCustomerByAddressId(connectionStringActivePosRead, minRow, AddressId.Value);
            }
            else
            {
                // Default: Retrieve customers from institution
                customers = customerService.GetCustomerInInstitution(connectionStringActivePosRead, minRow);
            }

            if (customers == null || customers.Count == 0)
            {
                // Return an error response if no customers are found
                return Json(new { Error = "No customer found" }, JsonRequestBehavior.AllowGet);
            }

            Customer firstCustomer = customers[0];
            ApiClientConvertToChmed16A apiClientConvertToChmed16A = new ApiClientConvertToChmed16A();

            // HappyPath CatB
            HttpResponseMessage responseConvertToChmed16A = await apiClientConvertToChmed16A.SendPostRequest(firstCustomer, Scenario.HappyPath, MedicineCategory.B);
            string happyPathResponseB = await responseConvertToChmed16A.Content.ReadAsStringAsync();

            // HappyPath CatA
            responseConvertToChmed16A = await apiClientConvertToChmed16A.SendPostRequest(firstCustomer, Scenario.HappyPath, MedicineCategory.A);
            string happyPathResponseA = await responseConvertToChmed16A.Content.ReadAsStringAsync();

            // HappyPath CatC
            responseConvertToChmed16A = await apiClientConvertToChmed16A.SendPostRequest(firstCustomer, Scenario.HappyPath, MedicineCategory.C);
            string happyPathResponseC = await responseConvertToChmed16A.Content.ReadAsStringAsync();

            // DateExpired CatA
            responseConvertToChmed16A = await apiClientConvertToChmed16A.SendPostRequest(firstCustomer, Scenario.DateExpired, MedicineCategory.A);
            string dateExpiredResponseA = await responseConvertToChmed16A.Content.ReadAsStringAsync();

            // Prepare JSON response
            var jsonResponse = new
            {
                FirstName = firstCustomer.FirstName,
                LastName = firstCustomer.LastName,
                HappyPathResponseB = happyPathResponseB,
                HappyPathResponseA = happyPathResponseA,
                HappyPathResponseC = happyPathResponseC,
                DateExpiredResponseA = dateExpiredResponseA
            };

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> OrderHubOrders()
        {
            //var environment = HostnameHelper.ExtractEnvironmentName();
            var hostname = HostnameHelper.GetCurrentHostname();
            CreateOrder order = new CreateOrder(hostname);
            //Get Pharmacy Info
            var pharmacyInfo = Helper.PharmacyInfo.GetPharmacyInfo();

            // Get Customer data
            List<Customer> customersInsurance = customerService.GetCustomerWithValidInsurance(connectionStringActivePosRead, 1);
            Customer customer = customersInsurance[0];
            CustomerViewModel customerViewModel = new CustomerViewModel(customer);

            // Get the response synchronously by waiting on the async method
            HttpResponseMessage responseConvertToChmed16A = await order.SendPostRequest(pharmacyInfo.PharmacyGln , customersInsurance, hostname);
            string response = await responseConvertToChmed16A.Content.ReadAsStringAsync();

            // Parse the JSON to extract data.id
            string dataId = string.Empty;
            using (JsonDocument doc = JsonDocument.Parse(response))
            {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("data", out JsonElement dataElement) &&
                    dataElement.TryGetProperty("id", out JsonElement idElement))
                {
                    dataId = idElement.GetString();
                }
            }

            // Data for web page
            ViewBag.PharmacyGln = pharmacyInfo.PharmacyGln;
            ViewBag.DataId = dataId;
            ViewBag.Response = response;
            ViewBag.CustomerViewModel = customerViewModel;

            return View();
        }

        // OrderHubOrdersJson
        [HttpGet]
        public async Task<JsonResult> OrderHubOrdersJson(
            OrderType orderType = OrderType.NoPrescription, 
            ApiType apiType = ApiType.Standard,
            bool isDebug=false)
        {
            //Set for debug purpuse, will be overide by real data if isDebug is false
            var pharmacyInfo = new PharmacyInfoViewModel
            {
                OuCode = "123456",
                OuId = 123456,
                PharmacyName = "Test Pharmacy",
                PharmacyVersion = "1.0",
                Language = "en",
                PharmacyGln = "7601001370944"
            };

            try
            {
                // Extract hostname and pharmacy info
                var hostname = HostnameHelper.GetCurrentHostname();
                if (!isDebug)
                {
                    pharmacyInfo = Helper.PharmacyInfo.GetPharmacyInfo();
                }
                CreateOrder order = new CreateOrder(hostname, apiType);


                // Get customer data depending on the scenario
                List<Customer> customers = new List<Customer>();

                Customer customerForRepetition = new Customer
                {
                    FirstName = "Test Samy",
                    LastName = "BLAEUER"
                };

                if (orderType == OrderType.WithRepetition) 
                {
                    // for Rx need to find an other way to get customer
                    customers.Add(customerForRepetition);
                }
                else
                {
                    customers = customerService.GetCustomerWithValidInsurance(connectionStringActivePosRead, 1);
                    if (customers == null || !customers.Any())
                    {
                        return Json(new { Error = "No customers with valid insurance found." }, JsonRequestBehavior.AllowGet);
                    }
                }
                Customer customer = customers[0];
                CustomerViewModel customerViewModel = new CustomerViewModel(customer);

                // Send the POST request asynchronously
                HttpResponseMessage responseConvertToChmed16A = await order.SendPostRequest(pharmacyInfo.PharmacyGln, customers, hostname, orderType);
                if (!responseConvertToChmed16A.IsSuccessStatusCode)
                {
                    return Json(new
                    {
                        Error = $"Failed to create order. Status Code: {responseConvertToChmed16A.StatusCode}"
                    }, JsonRequestBehavior.AllowGet);
                }

                // Get the response as a string
                string responseString = await responseConvertToChmed16A.Content.ReadAsStringAsync();

                // Deserialize the response string into a JToken
                JToken responseJson = JsonConvert.DeserializeObject<JToken>(responseString);

                // Extract data.id from the response
                string dataId = responseJson["data"]?["id"]?.ToString();

                // Clean the deeply nested "Response" data if necessary
                //var simplifiedResponse = responseJson["data"]; // Replace with meaningful data from the response if needed

                // Prepare the final JSON response
                var jsonResponse = new
                {
                    PharmacyGln = pharmacyInfo.PharmacyGln,
                    DataId = dataId,
                    OrderType = orderType.ToString(),
                    //Response = simplifiedResponse,  // Replace with meaningful response or remove if not needed
                    Customer = new
                    {
                        customerViewModel.FirstName,
                        customerViewModel.LastName
                        // customerViewModel.InsuranceNumber // Uncomment if InsuranceNumber is needed
                    }
                };

                return Json(jsonResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Catch any exceptions and return as part of the JSON response
                return Json(new { Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}