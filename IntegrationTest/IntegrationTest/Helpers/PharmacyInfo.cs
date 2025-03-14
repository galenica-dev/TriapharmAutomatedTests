using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegrationTest.Helpers
{
    public class PharmacyInfo
    {
        public string OuCode { get; set; }
        public int OuId { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyVersion { get; set; }
        public string Language { get; set; }
        public string PharmacyGln { get; set; }
        public DateTime? VersionDate { get; set; }

        /// <summary>
        /// Fetches the PharmacyInfo from the provided ServiceUrl and deserializes it into a PharmacyInfo object.
        /// </summary>
        /// <param name="serviceUrl">The URL of the service to fetch the JSON from.</param>
        /// <returns>A PharmacyInfo object populated with data from the JSON.</returns>
        public static async Task<PharmacyInfo> GetPharmacyInfoAsync(string serviceUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Fetch the JSON from the service
                    string json = await client.GetStringAsync(serviceUrl);

                    // Deserialize JSON into PharmacyInfo object
                    return JsonConvert.DeserializeObject<PharmacyInfo>(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while fetching PharmacyInfo: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
