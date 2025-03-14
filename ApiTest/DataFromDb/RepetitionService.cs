using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataFromDb
{
    public class RepetitionService
    {
        /// <summary>
        /// Generates the SQL query to fetch customer repetition data with a specified number of rows and patient ID.
        /// </summary>
        /// <param name="topRow">The maximum number of rows to fetch.</param>
        /// <param name="patientId">The Patient ID for filtering the data.</param>
        /// <returns>The SQL query as a string.</returns>
        private string GetCustomerRepetitionSql(int topRow, string patientId)
        {
            if (string.IsNullOrWhiteSpace(patientId))
                throw new ArgumentException("Patient ID cannot be null or empty.", nameof(patientId));

            return $@"
                SELECT TOP ({topRow})
                    ri.[Repetition_No],
                    ri.[Parent_Prescription_No],
                    ph.[PatientId],
                    ph.[Physician_ID],
                    ri.Parent_Sales_Order_No,
                    ri.Repetition_Type,
                    ri.LimitDate,
                    ri.DefaultEndDateRepetition,
                    ri.TotalQuantity,
                    ri.RemainingQuantity,
                    ph.PrescriptionNo,
                    sod.Sales_Order_Id,
                    sod.Sales_Order_Detail_Id,
                    sod.Item_Description,
                    sod.Item_Id
                FROM [ActivePos_read].[dbo].[IIIRepetition_Info] AS ri
                INNER JOIN [ActivePos_read].[dbo].[IIIPrescription_hdr] AS ph
                    ON ri.[Parent_Prescription_No] = ph.[PrescriptionNo]
                INNER JOIN [ActivePos_read].[dbo].[IIISales_Order_Detail] AS sod
                    ON ri.[Repetition_No] = sod.[Repetition_No]
                WHERE ph.[PatientId] = '{patientId}'";
        }

        /// <summary>
        /// Executes the query to fetch customer repetition data and returns a list of Repetition objects.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="query">The SQL query to execute.</param>
        /// <returns>A list of Repetition objects.</returns>
        private List<Repetition> QueryGetCustomerRepetition(string connectionString, string query)
        {
            var repetitions = new List<Repetition>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var repetition = new Repetition
                                {
                                    RepetitionNo = reader.GetGuid(reader.GetOrdinal("Repetition_No")).ToString(),
                                    ParentPrescriptionNo = reader.GetGuid(reader.GetOrdinal("Parent_Prescription_No")).ToString(),
                                    PatientId = reader.GetGuid(reader.GetOrdinal("PatientId")).ToString(),
                                    PhysicianId = reader.GetGuid(reader.GetOrdinal("Physician_ID")).ToString(),
                                    ParentSalesOrderNo = reader.GetInt32(reader.GetOrdinal("Parent_Sales_Order_No")).ToString(),
                                    RepetitionType = reader.GetInt32(reader.GetOrdinal("Repetition_Type")).ToString(),
                                    LimitDate = reader.IsDBNull(reader.GetOrdinal("LimitDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("LimitDate")),
                                    DefaultEndDateRepetition = reader.IsDBNull(reader.GetOrdinal("DefaultEndDateRepetition")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DefaultEndDateRepetition")),
                                    TotalQuantity = reader.IsDBNull(reader.GetOrdinal("TotalQuantity")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TotalQuantity")),
                                    RemainingQuantity = reader.IsDBNull(reader.GetOrdinal("RemainingQuantity")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("RemainingQuantity")),
                                    PrescriptionNo = reader.GetGuid(reader.GetOrdinal("PrescriptionNo")).ToString(),
                                    SalesOrderId = reader.GetInt32(reader.GetOrdinal("Sales_Order_Id")).ToString(),
                                    SalesOrderDetailId = reader.GetInt32(reader.GetOrdinal("Sales_Order_Detail_Id")).ToString(),
                                    ItemDescription = reader.GetString(reader.GetOrdinal("Item_Description")),
                                    ItemId = reader.GetInt32(reader.GetOrdinal("Item_Id")).ToString()
                                };

                                repetitions.Add(repetition);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                var repetitionError = new Repetition();
                repetitionError.Error = $"SQL error happens : {ex.Message}";
                repetitions.Add(repetitionError);
            }

            return repetitions;
        }

        /// <summary>
        /// Fetches a list of Repetition objects for a given customer.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="topRow">The maximum number of rows to fetch.</param>
        /// <param name="patientId">The Patient ID for filtering the data.</param>
        /// <returns>A list of Repetition objects.</returns>
        public List<Repetition> GetCustomerRepetition(string connectionString, int topRow, string patientId)
        {
            string query = GetCustomerRepetitionSql(topRow, patientId);
            return QueryGetCustomerRepetition(connectionString, query);
        }

        // Additional methods and logic can be implemented here.
    }
}
