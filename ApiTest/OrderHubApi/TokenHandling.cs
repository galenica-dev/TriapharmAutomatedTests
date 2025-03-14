using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHubApi
{
    public class TokenHandling
    {
        public TokenHandling()
        {
        }

        private string GetAccessTokenUsingCurl(string clientId, string clientSecret, string scope, string urlGetToken)
        {
            // Construct the curl command
            string curlCommand = $"curl -X POST -H \"Content-Type: application/x-www-form-urlencoded\" " +
                                 $"-d \"client_id={clientId}\" " +
                                 $"-d \"client_secret={clientSecret}\" " +
                                 $"-d \"scope={scope}\" " +
                                 $"-d \"grant_type=client_credentials\" " +
                                 $"\"{urlGetToken}\"";

            // Start a process to run the curl command
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + curlCommand)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            process.WaitForExit();

            // Capture and parse the output
            string result = process.StandardOutput.ReadToEnd();
            var jsonResult = System.Text.Json.JsonDocument.Parse(result);
            return jsonResult.RootElement.GetProperty("access_token").GetString();
        }

        public string GetAccessToken( string hostname, CreateOrder.ApiType apiType = CreateOrder.ApiType.Standard)
        {
            var config = ConfigurationManager.GetConfiguration(hostname);
            if (apiType == CreateOrder.ApiType.Rx)
                return GetAccessTokenUsingCurl(config.ClientIdRx, config.ClientSecretRx, config.Scope, config.UrlToGetToken);
            else
                return GetAccessTokenUsingCurl(config.ClientId, config.ClientSecret, config.Scope, config.UrlToGetToken);

        }
    }
}
