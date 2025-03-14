using System;
using System.Data.SqlClient;

namespace DataFromDb
{
    public static class CommonVar
    {
        /// <summary>
        /// Updates the bapd_value in the Bmc_application_default table based on the BAPK_key in the Bmc_application_key table.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="key">The BAPK_key to identify the record to update.</param>
        /// <param name="newValue">The new value to set for bapd_value.</param>
        /// <returns>An object representing the updated key-value pair or an error message.</returns>
        public static object UpdateValue(string connectionString, string key, string newValue)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string updateQuery = @"
                        UPDATE d
                        SET d.[bapd_value] = @newValue
                        FROM [Bmc_application_default] AS d
                        INNER JOIN [Bmc_application_key] AS k
                            ON k.[Bmc_application_key_ID] = d.[BAPD_bmc_application_key]
                        WHERE k.[BAPK_key] = @key;
                    ";

                    using (var command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@key", key);
                        command.Parameters.AddWithValue("@newValue", newValue);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Return the updated key-value pair
                            return new { Key = key, Value = newValue };
                        }
                        else
                        {
                            return new { Error = "No rows were updated. The key might not exist." };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new { Error = ex.Message };
                }
            }
        }

        /// <summary>
        /// Retrieves bapd_value from the Bmc_application_default table based on the BAPK_key in the Bmc_application_key table.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="key">The BAPK_key to identify the record.</param>
        /// <returns>An object representing the key-value pair or an error message.</returns>
        public static object GetValue(string connectionString, string key)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string selectQuery = @"
                        SELECT k.[BAPK_key], d.[bapd_value]
                        FROM [Bmc_application_key] AS k
                        INNER JOIN [Bmc_application_default] AS d
                            ON k.[Bmc_application_key_ID] = d.[BAPD_bmc_application_key]
                        WHERE k.[BAPK_key] = @key;
                    ";

                    using (var command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@key", key);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new
                                {
                                    Key = reader["BAPK_key"].ToString(),
                                    Value = reader["bapd_value"].ToString()
                                };
                            }
                            else
                            {
                                return new { Error = "Key not found." };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new { Error = ex.Message };
                }
            }
        }
    }
}
