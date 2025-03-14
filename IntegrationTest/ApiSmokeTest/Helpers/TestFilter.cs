using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiSmokeTest.Helpers
{
    public class TestFilter
    {
        public static async Task<string> GetGln(string domain) 
        {
            using HttpClient client = new HttpClient();

            string pharmacyInfoUrl = $"http://{domain}/WebApiClient/LocalService/PharmacyInfoJson";
            HttpResponseMessage pharmacyInfoResponse = await client.GetAsync(pharmacyInfoUrl);
            pharmacyInfoResponse.EnsureSuccessStatusCode();

            string pharmacyInfoContent = await pharmacyInfoResponse.Content.ReadAsStringAsync();
            JObject pharmacyInfoJson = JObject.Parse(pharmacyInfoContent);

            string pharmacyGln = pharmacyInfoJson["PharmacyGln"]?.ToString();

            return pharmacyGln;

        }

        public static async Task<bool> IsGreaterOrEqualVersion(string domain, string targetVersion)
        {
            string versionUrl = $"http://{domain}/WebApiClient/LocalService/PharmacyInfoJson";
            using HttpClient client = new HttpClient();
            HttpResponseMessage versionResponse = await client.GetAsync(versionUrl);
            versionResponse.EnsureSuccessStatusCode();

            string versionContent = await versionResponse.Content.ReadAsStringAsync();
            JObject versionJson = JObject.Parse(versionContent);

            string currentVersion = versionJson["PharmacyVersion"]?.ToString();

            return VersionCheck(currentVersion, targetVersion);
        }

        private static bool VersionCheck(string currentVersion, string targetVersion)
        {
            // Split the versions into parts
            var currentParts = currentVersion.Split('.');
            var targetParts = targetVersion.Split('.');

            // Ensure both versions have at least two parts (VV and M)
            if (currentParts.Length < 2 || targetParts.Length < 2)
            {
                throw new ArgumentException("Invalid version format. Versions must be in the format VV.M.YY.BUILD.");
            }

            // Parse the relevant parts
            int currentVV = int.Parse(currentParts[0]);
            int targetVV = int.Parse(targetParts[0]);

            int currentM = int.Parse(currentParts[1]);
            int targetM = int.Parse(targetParts[1]);

            // Compare VV
            if (currentVV > targetVV)
            {
                return true; // Current version is higher
            }
            else if (currentVV == targetVV)
            {
                // Compare M if VV is the same
                return currentM >= targetM;
            }

            return false; // Current version is lower
        }
    }
}
