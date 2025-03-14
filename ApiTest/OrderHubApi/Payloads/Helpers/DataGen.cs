using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHubApi.Payloads.Helpers
{
    public class DataGen
    {
        public static string GenSourceOrderId()
        {
            DateTime now = DateTime.UtcNow;
            int timestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return $"T{timestamp.ToString()}";
        }

        public static DateTime GetNowDateTime()
        {
            return DateTime.UtcNow;
        }

        public static DateTime GetNHoursBehind(int nbHourBehind)
        {
            return DateTime.UtcNow.AddHours(-nbHourBehind);
        }
    }
}
