using System;
using System.Net;

namespace ApiSmokeTest.Helpers
{
    public static class SutName
    {
        /// <summary>
        /// Gets the appropriate domain based on the current hostname.
        /// </summary>
        /// <returns>The domain name to use.</returns>
        public static string GetDomain()
        {
            // Get the current hostname of the device
            string currentHostName = Dns.GetHostName();

            // Check if the hostname matches the specific condition
            if (currentHostName.Equals("CGAL60280", StringComparison.OrdinalIgnoreCase))
            {
                return "swama704vm02.centralinfra.net";
                //return "swama705vm01.centralinfra.net";
                //return "swama888vm01.centralinfra.net";
            }

            // Default to localhost if the hostname does not match
            return "localhost";
        }
    }
}
