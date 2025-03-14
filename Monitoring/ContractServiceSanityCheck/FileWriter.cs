using System;
using System.IO;

namespace ContractServiceSanityCheck
{
    public class FileWriter
    {
        private readonly string _filePath;

        public FileWriter(string filePath)
        {
            _filePath = filePath;
            EnsureDirectoryExists();
        }

        private void EnsureDirectoryExists()
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void WriteLine(string content)
        {
            try
            {
                using (var writer = new StreamWriter(_filePath, append: true))
                {
                    string hostname = Environment.MachineName;
                    string timestampedContent = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss};{hostname};{content}";
                    writer.WriteLine(timestampedContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }


    }
}
