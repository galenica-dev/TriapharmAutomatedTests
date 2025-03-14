using Allure.NUnit;
using Allure.NUnit.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntegrationTest.SmokeTest
{
    [AllureNUnit]
    public class PharmacyInfo
    {
        [Test]
        [AllureOwner("Samy Kacem")]
        [AllureIssue("Bug : n/a")]
        [AllureTms("Test case : n/a")]
        [AllureLink("Story/Epic : n/a")]
        [AllureDescription("This test check if the ServiceMobile is working and we could get PharmacyInfo from it.")]
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
