
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using ApiSmokeTest.Sap;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using ApiSmokeTest.Helpers;

namespace ApiSmokeTest.Sap
{
    [AllureNUnit]
    public class PostXmlRecap
    {
        private const string TestUrl = "https://galenica-oc-development.it-cpi005-rt.cfapps.eu20.hana.ondemand.com/http/IF0002/01/V1/I08";
        private const string ValidSapClientId = "sb-50fa80e8-1824-40e5-890a-53c5ec024b48!b22101|it-rt-galenica-oc-development!b259";
        private const string ValidSapClientSecret = "8e44fb60-c86b-4af8-ad61-6790a3aa6d36$lVBqn8eE5CXcDBQzTYZJyZ7OsiAo8YdENSnMHAtwMP0=";
        private const string InvalidClientId = "invalid_client_id";
        private const string InvalidClientSecret = "invalid_client_secret";

        private string GetXmlBodyFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file at {filePath} does not exist.");
            }

            return File.ReadAllText(filePath);
        }

        [Test]
        [AllureOwner("Samy Kacem")]
        [AllureIssue("OCTP-7614")]
        [AllureTms("OCTP-7835")]
        public async Task SapPostXmlWithAuthAsync_ValidCredentials_ReturnsSuccess()
        {
            // Arrange
            string xmlFilePath = @"C:\QaTools\Ressources\CashReport.xml"; // Provide the path to your XML file
            string validXmlBody = GetXmlBodyFromFile(xmlFilePath);
            Console.WriteLine($"Xml: {validXmlBody}");

            // Act
            string response = await ApiPost.SapPostXmlWithAuthAsync(TestUrl, ValidSapClientId, ValidSapClientSecret, validXmlBody);

            // Assert
            Assert.IsNotNull(response, "The response should not be null.");
            Assert.IsNotEmpty(response, "The response should not be empty.");
            Assert.That(response, Does.Contain("<status>Data received</status>"), "The response does not contain the expected status message.");

            Console.WriteLine($"Response: {response}");
        }
    }
}
