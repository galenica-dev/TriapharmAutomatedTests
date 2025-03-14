using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ApiSmokeTest.SalesOrder
{
    [AllureNUnit]
    public class WebShop
    {
        private async Task WebshopOrder(string orderType) 
        {
            using HttpClient client = new HttpClient();
            int nbRetry = 25;
            int delayBetweenRetries = 2000; // 2 seconds
            string domain = Helpers.SutName.GetDomain();

            // Pre-Step: Call PharmacyInfoJson API
            string pharmacyGln = await Helpers.TestFilter.GetGln(domain);
            if (string.IsNullOrWhiteSpace(pharmacyGln))
            {
                Assert.Ignore("The PharmacyGln value is empty or null. Skipping the test.");
                return;
            }

            Console.WriteLine($"PharmacyGln: {pharmacyGln}");

            // Step 1: Get the OrderId
            string orderHubUrl = $"http://{domain}/WebApiClient/RemoteService/OrderHubOrdersJson?orderType={orderType}";

            HttpResponseMessage orderHubResponse = await client.GetAsync(orderHubUrl);
            orderHubResponse.EnsureSuccessStatusCode(); // Ensure the response status code is 200

            string orderHubContent = await orderHubResponse.Content.ReadAsStringAsync();

            // Parse JSON response correctly
            JObject orderHubJson = JObject.Parse(orderHubContent);
            string dataId = orderHubJson["DataId"]?.ToString();
            Assert.IsNotNull(dataId, "The DataId should not be null.");

            // Step 2: Call the SalesOrder API with retry logic
            string salesOrderUrl = $"http://{domain}/WebApiClient/Db/GetSalesOrder?onlineOrderId={dataId}";

            HttpResponseMessage salesOrderResponse = null;
            string salesOrderContent = null;
            JArray salesOrderArray = null;
            bool isDataAvailable = false;

            for (int attempt = 1; attempt <= nbRetry; attempt++)
            {
                Console.WriteLine($"Attempt {attempt}: Calling SalesOrder API...");
                salesOrderResponse = await client.GetAsync(salesOrderUrl);

                if (salesOrderResponse.IsSuccessStatusCode)
                {
                    salesOrderContent = await salesOrderResponse.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(salesOrderContent))
                    {
                        try
                        {
                            salesOrderArray = JArray.Parse(salesOrderContent);

                            if (salesOrderArray.Count > 0)
                            {
                                isDataAvailable = true;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Attempt {attempt}: Failed to parse response. Error: {ex.Message}");
                        }
                    }
                }

                Console.WriteLine($"Attempt {attempt}: No data available. Retrying in {delayBetweenRetries / 1000} seconds...");
                await Task.Delay(delayBetweenRetries); // Wait delayBetweenRetries/1000 seconds before retrying
            }

            Assert.IsTrue(isDataAvailable, $"The SalesOrder API did not return data after {nbRetry} retries.");

            // Parse the first order and perform assertions
            JObject firstOrder = (JObject)salesOrderArray[0];

            // Step 3: Assert OnlineOrderId matches DataId
            string onlineOrderId = firstOrder["OnlineOrderId"]?.ToString();
            Assert.AreEqual(dataId, onlineOrderId, "The OnlineOrderId should match the DataId.");

            // Step 4: Assert Error key is null
            JToken error = firstOrder["Error"];
            Assert.IsTrue(error == null || error.Type == JTokenType.Null, "The Error key should be null.");

            // Step 5: Assert PendingType = 7 for orderType = WithRepetition
            if (orderType == "WithRepetition")
            {
                string pendingType = firstOrder["PendingType"]?.ToString();
                Assert.AreEqual("7", pendingType, "The PendingType should be 7 for orderType = WithRepetition.");
            }
        }

        [Test]
        [AllureOwner("Samy Kacem")]
        [AllureIssue("OCTP-7898")]
        [AllureTms("n/a")]
        public async Task WebshopOrder_NoPrescription()
        {
            await WebshopOrder("NoPrescription");
        }

        [Test]
        [AllureOwner("Samy Kacem")]
        [AllureIssue("OCTP-7899")]
        [AllureTms("n/a")]
        public async Task WebshopOrder_WithRepetition()
        {
            string targetVersion = "24.5";
            string domain = Helpers.SutName.GetDomain();
            bool isGreaterOrEqualVersion = await Helpers.TestFilter.IsGreaterOrEqualVersion(domain, targetVersion);

            if (!isGreaterOrEqualVersion)
            {
                Assert.Ignore($"The current version is smaller to target version: {targetVersion}. Skipping the test.");
            }
            else
                await WebshopOrder("WithRepetition");
        }
    }
}
