using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataFromDb
{
    public static class SupplierService
    {
        /// <summary>
        /// Retrieves a list of suppliers by name based on the search term.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="searchTerm">The search term for supplier names.</param>
        /// <returns>A list of Supplier objects.</returns>
        public static List<Supplier> GetSuppliersByName(string connectionString, string searchTerm)
        {
            var suppliers = new List<Supplier>();

            string query = @"
            SELECT a.[AD_name] AS [name],
                   adk.ADK_key AS [code],
                   a.Address_ID,
                   s.Supplier_ID
            FROM [Arizona].[dbo].[Supplier] AS s
            INNER JOIN [Arizona].[dbo].[Address] AS a
                ON s.[SUPP_address] = a.[Address_ID]
            RIGHT JOIN address_key AS adk
                ON a.Address_ID = adk.ADK_address
            WHERE s.SUPP_address_category IS NOT NULL
              AND a.[AD_name] LIKE @searchTerm
              AND s.SUPP_active = 1
              AND adk.ADK_type = 1
            ORDER BY a.[AD_name];
        ";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suppliers.Add(new Supplier
                            {
                                Name = reader["name"].ToString(),
                                Code = reader["code"].ToString(),
                                AddressID = Convert.ToInt32(reader["Address_ID"]),
                                SupplierID = Convert.ToInt32(reader["Supplier_ID"])
                            });
                        }
                    }
                }
            }

            return suppliers;
        }
    }
}
