using System;
using System.IO;

namespace OrderHubApi
{
    public class LogEvent
    {
        private readonly string _eventName;
        private readonly string _folder;

        // Constructor that takes eventName and folder as parameters
        public LogEvent(string eventName, string folder)
        {
            _eventName = eventName;
            _folder = folder;
        }

        // Method to write the log message
        public void WriteLog(string message)
        {
            try
            {
                // Check if the directory exists; if not, create it
                if (!Directory.Exists(_folder))
                {
                    Directory.CreateDirectory(_folder);
                }

                // Generate the log file path (e.g., "CreateOrder_Log_yyyyMMdd.txt")
                string logFilePath = Path.Combine(_folder, _eventName + ".txt");

                // Write the log message to the file, appending to the existing file
                using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                // In case of an error, log the exception message to the console (or handle it as needed)
                Console.WriteLine($"Error logging event: {ex.Message}");
            }
        }
    }
}
