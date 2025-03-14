using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class EnvItem
    {
        public static List<string> GetServerList()
        {
            List<string> serversMonitoring = new List<string>();
            serversMonitoring.AddRange(GetServerListPilot());
            serversMonitoring.AddRange(GetServerListNPlus2());
            serversMonitoring.AddRange(GetServerListNPlus1Business());
            serversMonitoring.AddRange(GetServerListNPlus1Automation());

            return serversMonitoring;
        }

        public static List<string> GetServerListPilot() 
        {
            // Define the list of server paths
            List<string> serversMonitoring = new List<string>
            {
                "swama704vm02.centralinfra.net"
            };

            return serversMonitoring;
        }

        public static List<string> GetServerListNPlus2()
        {
            // Define the list of server paths
            List<string> serversMonitoring = new List<string>
            {
                "swcvi506vm01.centralinfra.net",
                "swsun007vm01.centralinfra.net",
                "swsun008vm01.centralinfra.net"
            };

            return serversMonitoring;
        }

        public static List<string> GetServerListNPlus1Business()
        {
            // Define the list of server paths
            List<string> serversMonitoring = new List<string>
            {
                "swama705vm01.centralinfra.net",
                "swama707vm01.centralinfra.net",
                "swcvi503vm01.centralinfra.net",
                "swcvi504vm01.centralinfra.net",
                "swsun004vm01.centralinfra.net",
                "swsun006vm01.centralinfra.net"
            };

            return serversMonitoring;
        }

        public static List<string> GetServerListNPlus1Automation()
        {
            // Define the list of server paths
            List<string> serversMonitoring = new List<string>
            {
                "swama888vm01.centralinfra.net",
                "swcvi888vm01.centralinfra.net",
                "swsun888vm01.centralinfra.net"
            };

            return serversMonitoring;
        }

        public static bool IsCurrentUserAdmin()
        {
            return new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent())
                .IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
    }
}
