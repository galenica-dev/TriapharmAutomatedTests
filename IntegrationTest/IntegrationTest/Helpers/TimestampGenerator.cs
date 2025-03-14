using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTest.Helpers
{
    public static class TimestampGenerator
    {
        public static string GenerateRandomTimestamp(int minusDate)
        {
            // Get current date
            DateTime currentDate = DateTime.Now;

            // Subtract one day
            DateTime targetDate = currentDate.AddDays(-minusDate);

            // Generate random hour, minute, and second
            Random random = new Random();
            int randomHour = random.Next(0, 24);      // 0 to 23
            int randomMinute = random.Next(0, 60);   // 0 to 59
            int randomSecond = random.Next(0, 60);   // 0 to 59

            // Combine the target date with random time
            DateTime randomTimestamp = new DateTime(
                targetDate.Year,
                targetDate.Month,
                targetDate.Day,
                randomHour,
                randomMinute,
                randomSecond
            );

            // Return the generated timestamp in ISO 8601 format
            return randomTimestamp.ToString("yyyy-MM-ddTHH:mm:ss");
        }
    }
}
