using Allure.NUnit.Attributes;
using Allure.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationTest.Helpers;

namespace IntegrationTest.Sap
{
    [AllureNUnit]
    public class PostXmlRecap
    {
        private const string TestUrl = "https://galenica-oc-development.it-cpi005-rt.cfapps.eu20.hana.ondemand.com/http/IF0002/01/V1/I08";
        private const string ValidSapClientId = "sb-50fa80e8-1824-40e5-890a-53c5ec024b48!b22101|it-rt-galenica-oc-development!b259";
        private const string ValidSapClientSecret = "8e44fb60-c86b-4af8-ad61-6790a3aa6d36$lVBqn8eE5CXcDBQzTYZJyZ7OsiAo8YdENSnMHAtwMP0=";
        private const string InvalidClientId = "invalid_client_id";
        private const string InvalidClientSecret = "invalid_client_secret";

        [Test]
        [AllureOwner("Samy Kacem")]
        [AllureIssue("Bug : OCTP-7614", "https://galenica.atlassian.net/browse/OCTP-7614")]
        [AllureTms("Test case : OCTP-7835", "https://galenica.atlassian.net/browse/OCTP-7835")]
        [AllureLink("Story/Epic : OCTP-7346", "https://galenica.atlassian.net/browse/OCTP-7346")]
        [AllureDescription("This test check if SAP service for cash report is answering.")]
        public async Task SapPostXmlWithAuthAsync_ValidCredentials_ReturnsSuccess()
        {
            string creationDate = TimestampGenerator.GenerateRandomTimestamp(1);
            string valueDate = TimestampGenerator.GenerateRandomTimestamp(2);   
            string startDate = TimestampGenerator.GenerateRandomTimestamp(3);
            string serviceUrl = $"http://{Helpers.SutName.GetDomain()}/WebApiClient/LocalService/PharmacyInfoJson";
            PharmacyInfo pharmacyInfo = await PharmacyInfo.GetPharmacyInfoAsync(serviceUrl);
            // Arrange
            string validXmlBody = $@"
                    <?xml version=""1.0"" encoding=""utf-16""?>
                    <ReceiptSummaryExport xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                                          xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
	                    <Header>
		                    <CreationDate>{creationDate}</CreationDate>
		                    <EndPeriod>{valueDate}</EndPeriod>
		                    <Number>2983</Number>
		                    <OrganizationalUnit>{pharmacyInfo.OuCode}</OrganizationalUnit>
		                    <PharmacyName>{pharmacyInfo.PharmacyName}</PharmacyName>
		                    <PosVersion>Version - Branch</PosVersion>
		                    <StartPeriod>{startDate}</StartPeriod>
		                    <Subsidiary>{pharmacyInfo.OuId}</Subsidiary>
		                    <SubsidiaryCode>{SutName.GetBrandFromDomain().ToUpper()}</SubsidiaryCode>
		                    <ValueDate>{valueDate}</ValueDate>
	                    </Header>
                    </ReceiptSummaryExport>
                ";
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
