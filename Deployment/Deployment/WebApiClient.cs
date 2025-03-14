using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Deployment.Helpers;

namespace Deployment
{
    public static class WebApiClient
    {
        public static void DeployFolder(List<string> servers)
        {
            string sourceFolder = @"C:\inetpub\wwwroot\WebApiClient";

            foreach (string server in servers)
            {
                string destinationFolder = $@"\\{server}\c$\inetpub\wwwroot\WebApiClient";

                // Delete destination folder content
                FileSys.DeleteFolderContent(destinationFolder);

                // Copy content from source to destination
                FileSys.CopyFolderContent(sourceFolder, destinationFolder);

                Console.ForegroundColor = ConsoleColor.Green;
                LogConsole.Log($"Deployment to server {server} completed.");
                Console.ResetColor();
            }
        }        

        public static async Task TestDeploymentAsync(List<string> servers)
        {
            using (HttpClient client = new HttpClient())
            {
                foreach (string server in servers)
                {
                    string url = $"http://{server}/WebApiClient/LocalService/PharmacyInfoJson";
                    LogConsole.Log($"Start to test deployment {server}");

                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string responseData = await response.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(responseData);

                        if (json.ContainsKey("PharmacyGln"))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            LogConsole.Log($"Server {server}: does have a key PharmacyGln in payload => Test passed");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            LogConsole.Log($"Server {server}: does not have a key PharmacyGln in payload' => Test failed");
                            Console.ResetColor();
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
