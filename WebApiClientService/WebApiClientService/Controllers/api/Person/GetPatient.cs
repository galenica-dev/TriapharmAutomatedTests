using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApiClientService.Controllers.Api.Person
{
    [ApiController]
    [Route("api/person/get-patient")]
    public class GetPatient : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public GetPatient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{patientGuid}")]
        public async Task<IActionResult> FindPatientByPatientGuid(string patientGuid)
        {
            try
            {
                // Define the external API URL
                string externalApiUrl = $"http://localhost:5001/api/person/get-patient/{patientGuid}";

                // Send a GET request
                HttpResponseMessage response = await _httpClient.GetAsync(externalApiUrl);

                // Ensure a successful response
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
                        error = $"Failed to retrieve data. Status code: {response.StatusCode}"
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
}
