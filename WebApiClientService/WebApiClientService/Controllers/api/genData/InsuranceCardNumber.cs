using DataGenerator;
using Microsoft.AspNetCore.Mvc;

namespace WebApiClientService.Controllers.Api.GenData
{
    [ApiController]
    [Route("api/gen-data/insurance-card-number")]
    public class InsuranceCardNumber : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInsuranceCardNumber()
        {
            InsuranceCardNumberGenerator insuranceCardNumberGenerator = new InsuranceCardNumberGenerator();

            return Ok(new
            {
                insuranceCardNumber = insuranceCardNumberGenerator.GenerateCardNumber(),
                timestamp = DateTime.UtcNow
            });
        }
    }
}
