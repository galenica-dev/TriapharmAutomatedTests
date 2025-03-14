using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApiClient.Controllers
{
    public class RemoteCmdController : Controller
    {
        // GET: RemoteCmd/Index
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ExecuteCmd(string cmd)
        {
            var result = await ExecuteCommandAsync(cmd);

            return Json(new
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
                process.StartInfo.FileName = "cmd.exe";  // The executable path, e.g., ContractServiceSanityCheck.exe
                process.StartInfo.Arguments = $"/c {cmd}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;  // Required to redirect output
                process.StartInfo.CreateNoWindow = true;  // Hides the console window
                process.StartInfo.WorkingDirectory = @"C:\QaTools"; // Set the desired working directory here

                process.Start();

                // Asynchronously read both the output and error streams
                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();

                await Task.WhenAll(outputTask, errorTask);  // Wait for both reads to complete

                output = outputTask.Result;
                error = errorTask.Result;

                await Task.Run(() => process.WaitForExit());
            }

            return (output, error);
        }

    }
}
