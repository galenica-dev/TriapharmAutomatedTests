using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApiClientService.Controllers.Remote
{
    [ApiController]
    [Route("remote/cmd")]
    public class CmdController : ControllerBase
    {
        [HttpPost("execute")]
        public async Task<IActionResult> ExecuteCmd([FromBody] CommandRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Cmd))
            {
                return BadRequest(new { error = "Command cannot be empty" });
            }

            var result = await ExecuteCommandAsync(request.Cmd);

            return Ok(new
            {
                output = result.Output,
                error = result.Error
            });
        }

        private async Task<(string Output, string Error)> ExecuteCommandAsync(string cmd)
        {
            var output = string.Empty;
            var error = string.Empty;

            using (var process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {cmd}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WorkingDirectory = @"C:\QaTools";

                process.Start();

                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();

                await Task.WhenAll(outputTask, errorTask);

                output = outputTask.Result;
                error = errorTask.Result;

                await Task.Run(() => process.WaitForExit());
            }

            return (output, error);
        }
    }

    public class CommandRequest
    {
        public string Cmd { get; set; }
    }
}
