using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTest.Helpers
{
    public static class ApiBearerToken
    {
        public static async Task<string> GetSapTokenAsync(string clientId, string clientSecret)
        {
            const string tokenUrl = "https://galenica-oc-development.authentication.eu20.hana.ondemand.com/oauth/token";

            using (var httpClient = new HttpClient())
            {
                // Prepare the request body
                var requestBody = "grant_type=client_credentials";
                var content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

                // Set the Authorization header with Basic Authentication
                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                // Make the POST request
                var response = await httpClient.PostAsync(tokenUrl, content);

                // Log the response status
                Console.WriteLine($"HTTP Status: {response.StatusCode}");

                // Log the response content for debugging
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {json}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve token. HTTP Status: {response.StatusCode}, Content: {json}");
                }

                // Parse the response JSON
                var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(json);

                // Return the access token
                return tokenResponse?.AccessToken ?? throw new Exception("Access token is missing in the response.");
            }
        }

        // Class to map the token response JSON
        private class TokenResponse
        {
            [System.Text.Json.Serialization.JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("token_type")]
            public string TokenType { get; set; }

            [System.Text.Json.Serialization.JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }
        }

    }
}
