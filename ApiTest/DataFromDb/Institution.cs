using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DataFromDb
{
    public static class Institution
    {
        // Static method to fetch institutions from the database
        public static List<(string AdName, string ContractNumber, Guid UGuid)> GetInstitutions(string connectionString, int topRow)
        {
            // Define the SQL query
            string query = $@"
            SELECT TOP {topRow} ad_name, PHPR_contract_number, PH_prescriber_GUID
            FROM Arizona.dbo.PH_prescriber
            JOIN arizona.dbo.address
            ON phpr_address = address_id
            JOIN arizona.[dbo].[PH_prescriber_role]
            ON [PHPRO_PH_prescriber] = PH_prescriber_GUID
            JOIN ActivePos_read.dbo.CommonVar
            ON [value] LIKE [PHPRO_PHGD_CODES]
            AND [key] = 'cvPHEMSPrescriberRoleMmr'
            WHERE PHPR_status = 1
            AND PHPR_contract_number IS NOT NULL
            ";

            var institutions = new List<(string AdName, string ContractNumber, Guid UGuid)>();

            // Create a SqlConnection using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand object
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the query and retrieve the data using SqlDataReader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if the query returned any rows
                            if (reader.HasRows)
                            {
                                // Loop through the result set
                                while (reader.Read())
                                {
                                    // Add each institution to the list as a tuple
                                    institutions.Add((reader.GetString(0), reader.GetString(1), reader.GetGuid(2)));
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that may have occurred
                    //This will only work in console mode
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return institutions;
        }

        // Static method to display a list of institutions
        public static void DisplayInstitutions(List<(string AdName, string ContractNumber, Guid UGuid)> institutions)
        {
            foreach (var institution in institutions)
            {
                Console.WriteLine($"{institution.ContractNumber}: {institution.AdName} ({institution.UGuid})");
            }
        }

        public static Guid GetValidInstitutionGuid(List<(string AdName, string ContractNumber, Guid UGuid)> institutions) 
        {
            return institutions[0].UGuid;
        }
    }
}
