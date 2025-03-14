using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Allure.NUnit;
using Allure.Net.Commons;
using Allure.NUnit.Attributes;

namespace ApiSmokeTest
{
    [AllureNUnit]
    public class PharmacyInfoTests
    {
        [Test]
        //[AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("Samy Kacem")]
        //[AllureLink("Website", "https://dev.example.com/")]
        //[AllureIssue("UI-123")]
        //[AllureTms("TMS-456")]
        public async Task ShouldContainPharmacyGlnInResponse()
        {
            string ServiceUrl = $"http://{Helpers.SutName.GetDomain()}/WebApiClient/LocalService/PharmacyInfoJson";

            // Arrange: Create an HttpClient to call the service
            using HttpClient client = new HttpClient();

            // Act: Send a GET request to the service
            HttpResponseMessage response = await client.GetAsync(ServiceUrl);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            string responseContent = await response.Content.ReadAsStringAsync();

            // Parse the JSON payload
            JObject jsonResponse = JObject.Parse(responseContent);

            // Assert: Check if the JSON contains the key "PharmacyGln"
            Assert.IsTrue(jsonResponse.ContainsKey("PharmacyGln"), "Response JSON does not contain the key 'PharmacyGln'");

            // Optionally, log the value for debugging
            string pharmacyGln = jsonResponse["PharmacyGln"]?.ToString();
            TestContext.WriteLine($"PharmacyGln: {pharmacyGln}");
        }
    }
}
