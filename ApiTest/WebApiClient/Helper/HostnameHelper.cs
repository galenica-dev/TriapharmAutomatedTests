using OrderHubApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiClient.Helper
{
    public static class HostnameHelper
    {
        /// <summary>
        /// Gets the current hostname in lowercase.
        /// </summary>
        /// <returns>The current hostname.</returns>
        public static string GetCurrentHostname()
        {
            return System.Net.Dns.GetHostName().ToLower();
        }

        /// <summary>
        /// Extracts the environment name from the hostname based on specific rules.
        /// </summary>
        /// <returns>The extracted environment name.</returns>
        public static EnvironmentName ExtractEnvironmentName()
        {
            string hostname = GetCurrentHostname();

            if (hostname.StartsWith("sw") && hostname.Length >= 5)
            {
                // Extract characters from the 3rd to the 8th position
                string extracted = hostname.Substring(2, 3).ToLower();

                if (extracted == "sun")
                {
                    return EnvironmentName.Sun;
                }
                else if (extracted == "cvi")
                {
                    return EnvironmentName.Cvi;
                }
            }

            // Default to Ama if hostname is shorter or does not match the rule
            return EnvironmentName.Ama;
        }
    }
}