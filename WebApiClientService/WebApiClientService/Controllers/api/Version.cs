using Microsoft.AspNetCore.Mvc;

namespace WebApiClientService.Controllers.Api
{
    [ApiController]
    [Route("api/version")]
    public class Version : ControllerBase // Keep "Version" as class name, but must inherit ControllerBase
    {
        [HttpGet]
        public IActionResult GetVersion()
        {
            return Ok(new { 
                version = "1.0.0.2",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
