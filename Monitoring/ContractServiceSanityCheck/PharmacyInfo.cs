using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ContractServiceSanityCheck
{
    public static class PharmacyInfo
    {
        public static async Task<string> GetPharmacyVersion()
        {

            string serverHostname = "localhost";
            
            if (System.Net.Dns.GetHostName().ToLower() == "cgal60280") 
            {
                serverHostname = "swama704vm02.centralinfra.net";
            }
            string apiUrl = $"http://{serverHostname}/WebApiClient/LocalService/PharmacyInfoJson";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonResponse);

                    // Retrieve the PharmacyVersion value
                    string pharmacyVersion = jsonObject["PharmacyVersion"]?.ToString();

                    return pharmacyVersion;
                }
                else
                {
                    //throw new HttpRequestException($"Request to {apiUrl} failed with status code {response.StatusCode}");
                    return "error";
                }
            }
        }
    }
}
