using DataGenerator;
using Microsoft.AspNetCore.Mvc;

namespace WebApiClientService.Controllers.Api.GenData
{
    [ApiController]
    [Route("api/gen-data/ahv")]
    public class Ahv : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAhv()
        {
            AHVAVSNumberGenerator avs = new AHVAVSNumberGenerator();

            return Ok(new
            {
                ahv = avs.GenerateAHVAVSNumber(),
                timestamp = DateTime.UtcNow
            });
        }
    }
}
