using Deployment.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Deployment
{
    public static class WebApiClientService
    {
        

        private static void CopyBinaries(List<string> servers, string source, string destination)
        {
            //string sourceFolder = @"C:\QaTools\WebApiClientServiceUpdates";

            foreach (string server in servers)
            {
                string destinationFolder = $@"\\{server}\c$\QaTools\{destination}";

                // Delete destination folder content
                FileSys.DeleteFolderContent(destinationFolder);
                LogConsole.Log($"Delete on {server} folder content {destinationFolder} completed.");

                // Copy content from source to destination
                FileSys.CopyFolderContent(source, destinationFolder);

                Console.ForegroundColor = ConsoleColor.Green;
                LogConsole.Log($"Deployment to server {server} completed.");
                Console.ResetColor();
            }
        }

        private static void DeleteBinaries(List<string> servers, string folderPath) 
        {
            foreach (string server in servers)
            {
                FileSys.DeleteFolderContent($@"\\{server}\c$\QaTools\{folderPath}");
                LogConsole.Log($"Delete on {server} folder content {folderPath} completed.");
            }
        }

        public static void CopyServiceBinaries(List<string> servers)
        {
            string sourceFolder = @"C:\QaTools\WebApiClientServiceUpdates";
            CopyBinaries(servers, sourceFolder, "WebApiClientServiceUpdates");

        }

        public static void CopyServiceUpdaterBinaries(List<string> servers)
        {
            string sourceFolder = @"C:\QaTools\WebApiClientServiceUpdater";
            CopyBinaries(servers, sourceFolder, "WebApiClientServiceUpdater");

        }

        public static async Task SendRemoteCmd(List<string> servers, string cmd, bool isService = true)
        {
            string urlIis = "/WebApiClient/RemoteCmd/ExecuteCmd";
            string urlService = ":8080/remote/cmd/execute";


            using (HttpClient client = new HttpClient())
            {
                foreach (string server in servers)
                {
                    string url = $"http://{server}{(isService ? urlService : urlIis)}";
                    LogConsole.Log($"Start sending remote command: {cmd} to server: {server}");

                    try
                    {
                        HttpResponseMessage response;

                        if (isService)
                        {
                            // Escape double quotes for JSON
                            string escapedCmd = cmd.Replace("\"", "\\\"");
                            var payload = new { cmd };
                            string jsonContent = JsonConvert.SerializeObject(payload);
                            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            response = await client.PostAsync(url, content);
                        }
                        else
                        {
                            string fullUrl = $"{url}?cmd={Uri.EscapeDataString(cmd)}";
                            var content = new StringContent("", Encoding.UTF8, "application/json");

                            response = await client.PostAsync(fullUrl, content);
                        }

                        string responseContent = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            LogConsole.Log($"Server {server}: Command sent successfully.");
                            LogConsole.Log($"Response Payload: {responseContent}");
                        }
                        else
                        {
                            LogConsole.Log($"Server {server}: Failed to execute command. Status: {response.StatusCode}");
                            LogConsole.Log($"Response Payload: {responseContent}");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogConsole.Log($"Server {server}: Failed to connect or validate response. Error: {ex.Message}");
                    }
                }
            }
        }
    }
}
