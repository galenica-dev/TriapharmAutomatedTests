using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApiClientService.Controllers.Api.Person
{
    [ApiController]
    [Route("api/person/create-patient")]
    public class CreatePatient : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CreatePatient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPatient([FromBody] CreatePatientRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    timestamp = DateTime.UtcNow,
                    error = "Invalid request body."
                });
            }

            try
            {
                // Convert request object to JSON
                var jsonContent = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Define external API URL
                string externalApiUrl = "http://localhost:5001/api/person/create-patient";

                // Send a POST request
                HttpResponseMessage response = await _httpClient.PostAsync(externalApiUrl, content);

                // Handle response
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    return Ok(new
                    {
                        timestamp = DateTime.UtcNow,
                        data = responseData
                    });
                }
                else
                {
                    return StatusCode((int)response.StatusCode, new
                    {
                        timestamp = DateTime.UtcNow,
                        error = $"Failed to create patient. Status code: {response.StatusCode}"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    timestamp = DateTime.UtcNow,
                    error = $"An error occurred: {ex.Message}"
                });
            }
        }
    }

    public class CreatePatientRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street1 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string CountryIso { get; set; }
        public int Language { get; set; }
        public string DateOfBirth { get; set; } // Format: YYYY-MM-DD
    }
}
