using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataFromDb;
using static eRxCHMED16Service.ApiClientConvertToChmed16A;

namespace eRxCHMED16Service
{
    public class ApiClientConvertToChmed16A
    {
        private readonly HttpClient _client;

        public ApiClientConvertToChmed16A()
        {
            _client = new HttpClient();
            // Set up the headers
            _client.DefaultRequestHeaders.Add("accept", "text/plain");
            _client.DefaultRequestHeaders.Add("HCI-CustomerId", "123456");
            _client.DefaultRequestHeaders.Add("HCI-Index", "hospindex");
            _client.DefaultRequestHeaders.Add("HCI-SoftwareOrgId", "123456");
            _client.DefaultRequestHeaders.Add("HCI-SoftwareOrg", "AcmeCorporation");
            _client.DefaultRequestHeaders.Add("HCI-Software", "AcmeCorpCisHospitalXy");
            _client.DefaultRequestHeaders.Add("HCI-SubCatalogId", "D683");
            _client.DefaultRequestHeaders.Add("HCI-WholesalerGln", "7601001401297");
        }

        public enum Scenario
        {
            HappyPath,
            DateExpired
        }

        public enum MedicineCategory 
        { 
            A,
            B,
            C
        }

        public async Task<HttpResponseMessage> SendPostRequest(Customer customer, Scenario scenario, MedicineCategory medicineCategory = MedicineCategory.B)
        {
            var url = "https://documedis.hcisolutions.ch/2020-01/api/converters/convertToChmed16A?compressed=true";
            var payload = string.Empty;

            // Create the payload
            if (scenario == Scenario.DateExpired)
            {
                payload = PayloadDateExpired(customer, medicineCategory);
                //Console.WriteLine($"Payload: {payload}");
            }
            else 
            {
                payload = PayloadHappyPath(customer, medicineCategory);
                //Console.WriteLine($"Payload: {payload}");
            }


            // Convert the payload to StringContent and set the content type
            var content = new StringContent(payload, Encoding.UTF8, "text/plain");

            // Send the POST request and return the response
            HttpResponseMessage response = await _client.PostAsync(url, content);

            return response;
        }

        private string PayloadHappyPath(Customer customer, MedicineCategory medicineCategory = MedicineCategory.B)
        {
            var medicineId = HandelMedicineCategory(medicineCategory);

            //Carefull BirthDate is mandatory
            return $@"CHMED16A0 {{""Patient"":{{""FName"":""{customer.FirstName}"",""LName"":""{customer.LastName}"",""BDt"":""{customer.DateOfBirth.ToString("yyyy-MM-dd")}"",""Gender"":{customer.Sex},""Street"":""{customer.Address1}"",""Zip"":""{customer.ZipCode}"",""City"":""{customer.City}"",""Phone"":"""",""Rcv"":""""}},""Medicaments"":[{{""Id"":""{medicineId}"",""IdType"":3,""Pos"":[{{""D"":[1.0,1.0,1.0,0.0]}}],""Unit"":""Stk"",""AppInstr"":"""",""Roa"":""PO"",""Subs"":1,""NbPack"":1}}],""PSchema"":"""",""MedType"":3,""Id"":""18934d92-f1e8-4b8d-b9e9-3eeaf27285ca"",""Auth"":""7601000399434"",""Zsr"":""N291102"",""Dt"":""{DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")}"",""Rmk"":""""}}";
        }

        private string PayloadDateExpired(Customer customer, MedicineCategory medicineCategory = MedicineCategory.B)
        {
            var medicineId = HandelMedicineCategory(medicineCategory);

            //Carefull BirthDate is mandatory
            return $@"CHMED16A0 {{""Patient"":{{""FName"":""{customer.FirstName}"",""LName"":""{customer.LastName}"",""BDt"":""{customer.DateOfBirth.ToString("yyyy-MM-dd")}"",""Gender"":{customer.Sex},""Street"":""{customer.Address1}"",""Zip"":""{customer.ZipCode}"",""City"":""{customer.City}"",""Phone"":"""",""Rcv"":""""}},""Medicaments"":[{{""Id"":""{medicineId}"",""IdType"":3,""Pos"":[{{""D"":[1.0,1.0,1.0,0.0]}}],""Unit"":""Stk"",""AppInstr"":"""",""Roa"":""PO"",""Subs"":1,""NbPack"":1}}],""PSchema"":"""",""MedType"":3,""Id"":""18934d92-f1e8-4b8d-b9e9-3eeaf27285ca"",""Auth"":""7601000399434"",""Zsr"":""N291102"",""Dt"":""{DateTimeOffset.Now.AddYears(-2).ToString("yyyy-MM-ddTHH:mm:sszzz")}"",""Rmk"":""""}}";
        }

        private string HandelMedicineCategory(MedicineCategory medicineCategory) 
        {
            var medicineId = "2181276"; //default Dafalgan 1g catB
            if (medicineCategory == MedicineCategory.A)
                medicineId = "1793959"; //Augmentin 1g catA
            else if (medicineCategory == MedicineCategory.C)
                medicineId = "1336653"; //Dafalgan 0.5g catC

            return medicineId;
        }

    }
}
