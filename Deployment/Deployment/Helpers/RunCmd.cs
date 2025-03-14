using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class RunCmd
    {
        private const int DefaultTimeoutInSeconds = 600; // Timeout value in seconds

        public static async Task<bool> Execute(string serverName, string cmd, TimeSpan? timeout = null)
        {
            string baseUrl = $"http://{serverName}/WebApiClient/RemoteCmd/ExecuteCmd";
            string encodedCmd = Uri.EscapeDataString(cmd);

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = timeout ?? TimeSpan.FromSeconds(DefaultTimeoutInSeconds);
                try
                {
                    //Console.WriteLine($"Sending POST request {cmd}...");
                    LogConsole.Log($"Sending POST request {cmd}...");

                    // Create the full URL with the encoded command
                    string url = $"{baseUrl}?cmd={encodedCmd}";

                    // Send the request
                    HttpResponseMessage response = await client.PostAsync(url, null);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine($"Request {cmd} was successful (HTTP 200).");
                        return true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        LogConsole.Log($"Failed {cmd}. HTTP Status: {response.StatusCode}");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    LogConsole.Log($"Exception occurred while trying {cmd}. Error: {ex.Message}");
                    Console.ResetColor();
                }
            }

            return false;
        }

        public static async Task<bool> KillTaskAsync(string serverName, string exeName, TimeSpan? timeout = null)
        {
            return await Execute(serverName, $"taskkill /IM {exeName} /F", timeout);
        }
    }
}
