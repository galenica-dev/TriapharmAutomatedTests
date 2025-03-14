using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ContractServiceSanityCheck
{
    public class WebServiceRepository
    {
        private string _connectionString;
        private List<string> _excludedUris;

        public WebServiceRepository(string connectionString)
        {
            _connectionString = connectionString;
            LoadExcludedUris(@"C:\Temp\ContractServiceSanityCheck_ExcludedUris.txt"); // Load exclusions from the file
        }

        private void LoadExcludedUris(string filePath)
        {
            // Read all lines from the exclusion file and process each line to ignore comments
            if (File.Exists(filePath))
            {
                _excludedUris = File.ReadAllLines(filePath)
                                    .Select(line => line.Split(';')[0].Trim()) // Take part before semicolon and trim
                                    .Where(line => !string.IsNullOrEmpty(line))
                                    .ToList();
            }
            else
            {
                _excludedUris = new List<string>(); // Empty list if file not found
            }
        }

        public List<WebService> GetWebServices(string pharmacyVersion, string monitorVersion)
        {
            var webServices = new List<WebService>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT [WebserviceId], [uri], [Login], [Password], [CompanyCode], [Code]
                    FROM [ActivePos_read].[dbo].[WebService]";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var webService = new WebService
                        {
                            WebserviceId = reader.GetGuid(reader.GetOrdinal("WebserviceId")),
                            Uri = reader.GetString(reader.GetOrdinal("uri")),
                            Login = reader.GetString(reader.GetOrdinal("Login")),
                            Password = reader.GetString(reader.GetOrdinal("Password")),
                            CompanyCode = reader.IsDBNull(reader.GetOrdinal("CompanyCode"))
                                          ? null
                                          : reader.GetString(reader.GetOrdinal("CompanyCode")),
                            Code = reader.GetString(reader.GetOrdinal("Code")),
                            TriaPharmVersion = pharmacyVersion,
                            ContractServiceSanityCheckVersion = monitorVersion
                        };

                        // Check if the URI or Code contains any excluded patterns, using case-insensitive comparison
                        if (_excludedUris.Any(exclusion =>
                              webService.Uri.IndexOf(exclusion, StringComparison.OrdinalIgnoreCase) >= 0 ||
                              webService.Code.IndexOf(exclusion, StringComparison.OrdinalIgnoreCase) >= 0))
                        {
                            continue;
                        }

                        webServices.Add(webService);
                    }
                }
            }

            return webServices;
        }
    }
}
