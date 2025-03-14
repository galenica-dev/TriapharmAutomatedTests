using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiSmokeTest.Helpers
{
    public static class ApiPost
    {
        /// <summary>
        /// Sends a POST request with an XML body to the specified SAP endpoint using OAuth Bearer Token authentication.
        /// </summary>
        /// <param name="url">The SAP endpoint URL.</param>
        /// <param name="clientId">The client ID for authentication.</param>
        /// <param name="clientSecret">The client secret for authentication.</param>
        /// <param name="xmlBody">The XML body to include in the POST request.</param>
        /// <returns>The response content as a string.</returns>
        public static async Task<string> SapPostXmlWithAuthAsync(string url, string clientId, string clientSecret, string xmlBody)
        {
            // Get the Bearer token
            string token = await ApiBearerToken.GetSapTokenAsync(clientId, clientSecret);

            using (var httpClient = new HttpClient())
            {
                // Extract the host from the URL
                var uri = new Uri(url);
                string host = uri.Host;

                // Set authorization and host headers
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                httpClient.DefaultRequestHeaders.Add("Host", host);

                // Prepare the XML content
                var content = new StringContent(xmlBody, Encoding.UTF8, "application/xml");

                // Make the POST request
                var response = await httpClient.PostAsync(url, content);

                // Log response status
                Console.WriteLine($"HTTP Status: {response.StatusCode}");

                // Read the response content
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"POST request failed. HTTP Status: {response.StatusCode}, Content: {responseContent}");
                }

                return responseContent;
            }
        }
    }
}
