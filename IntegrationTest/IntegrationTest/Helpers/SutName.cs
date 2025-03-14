using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTest.Helpers
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
                //return "swama704vm02.centralinfra.net";
                //return "swama707vm01.centralinfra.net";
                //return "swama705vm01.centralinfra.net";
                //return "swama888vm01.centralinfra.net";
                //return "swsun007vm01.centralinfra.net";
                //return "swsun007vm01.centralinfra.net";
                //return "swsun008vm01.centralinfra.net";
                return "swcvi504vm01.centralinfra.net";
            }

            // Default to localhost if the hostname does not match
            return "localhost";
        }

        /// <summary>
        /// Resolves the current hostname and extracts the substring from the 3rd to the 5th character.
        /// </summary>
        /// <returns>The substring from the 3rd to the 5th character of the hostname.</returns>
        public static string GetBrandFromDomain()
        {
            string currentHostName = System.Net.Dns.GetHostName().ToLower();
            Console.WriteLine($"Current Hostname: {currentHostName}");

            // Check if the hostname matches the specific condition
            if (currentHostName.Equals("cgal60280", StringComparison.OrdinalIgnoreCase))
            {
                //currentHostName = "swama704vm02";
                //currentHostName = "swama705vm01";
                //currentHostName = "swama707vm01";
                //currentHostName = "swama888vm01";
                //currentHostName = "swsun007vm01";
                //currentHostName = "swsun007vm01";
                //currentHostName = "swsun008vm01";
                currentHostName = "swcvi504vm01";

            }

            if (string.IsNullOrEmpty(currentHostName) || currentHostName.Length < 5)
            {
                throw new ArgumentException("The resolved hostname must have at least 5 characters.");
            }

            return currentHostName.Substring(2, 3); // Start at index 2 (3rd character) and take 3 characters
        }
    }
}
