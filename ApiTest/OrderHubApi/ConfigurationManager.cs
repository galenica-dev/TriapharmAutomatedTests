using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHubApi
{
     public class EnvironmentConfig
    {
        public string ClientId { get; set; }
        public string ClientIdRx { get; set; }
        public string ClientSecret { get; set; }
        public string ClientSecretRx { get; set; }
        public string Scope { get; set; }
        public string UrlToGetToken { get; set; }
        public string SubKey { get; set; }
        public string SubKeyRx { get; set; }
        public string UrlPostRequest { get; set; }
    }

    public static class ConfigurationManager
    {
        private static readonly Dictionary<string, EnvironmentConfig> HostnameConfigurations;
        
        static ConfigurationManager()
        {
            try
            {
                // Define the path to the configuration file
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hostnames.json");
                
                // Load and deserialize the configuration
                if (File.Exists(configPath))
                {
                    string jsonContent = File.ReadAllText(configPath);
                    HostnameConfigurations = JsonSerializer.Deserialize<Dictionary<string, EnvironmentConfig>>(
                        jsonContent, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }
                else
                {
                    throw new FileNotFoundException($"Configuration file not found at: {configPath}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                throw;
            }
        }

        public static EnvironmentConfig GetConfiguration(string hostname)
        {
            if (HostnameConfigurations.TryGetValue(hostname, out config))
            {
                return config;
            }
            throw new KeyNotFoundException($"Configuration for hostname '{hostname}' not found.");
        }
    }

}
