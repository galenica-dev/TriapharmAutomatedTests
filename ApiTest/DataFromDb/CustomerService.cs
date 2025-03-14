using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataFromDb
{
    public class CustomerService
    {
        #region SQLQueryBuilder
        // Instance method to construct the SQL query
        private string BaseSql(int topRow)
        {
            return $@"
            SELECT TOP ({topRow}) p.[AddressId]
                  ,p.[LastName]
                  ,p.[FirstName]
                  ,p.[Address1]
                  ,p.[Address2]
                  ,p.[ZipCode]
                  ,p.[City]
                  ,p.[AddressMainKey]
                  ,p.[CountryId]
                  ,p.[Sex]
                  ,p.[DateOfBirth]
                  ,p.[Language]
                  ,p.[Dead]
                  ,p.[AHVNumber]
                  ,p.[CustomerCard]
                  ,p.[Active]
                  ,p.[OmnichannelUUID]
                  ,p.[AddressSupplement]
	              ,c.Height
	              ,c.[Weight]
	              ,c.[EmployeeCardNumber]
	              ,c.[Institution]
            FROM [ActivePos_read].[dbo].[IIIPerson] AS p
            INNER JOIN [ActivePos_read].[dbo].[IIICustomerToPerson] AS ctp
            ON p.[AddressId] = ctp.AddressId
            INNER JOIN [ActivePos_read].[dbo].[IIICustomer] as c
            ON ctp.CustomerId = c.CustomerId
            ";
        }

        // Method for Customers with valid insurance
        private string CustomerWithValidInsuranceSql(int topRow)
        {
            return $@"
                SELECT TOP ({topRow}) p.[AddressId]
                      ,p.[LastName]
                      ,p.[FirstName]
                      ,i.[CoverCard_Number]
                      ,i.[Insurance_Valid_upto]
                      ,p.[DateOfBirth]
                      ,p.[Address1]
                      ,p.[Address2]
                      ,p.[ZipCode]
                      ,p.[City]
                      ,p.[CountryId]
	                  ,ins.[Insurance_Name]
                  FROM [ActivePos_read].[dbo].[IIIPerson] AS p
                  INNER JOIN [ActivePos_read].[dbo].[IIICustomerToPerson] AS ctp
                  ON p.[AddressId] = ctp.AddressId
                  INNER JOIN [ActivePos_read].[dbo].[IIICustomer] as c
                  ON ctp.CustomerId = c.CustomerId
                  INNER JOIN [IIIPatient_Insurance] as i
                  ON i.[CustomerId] = c.CustomerId
                  INNER JOIN [IIIInsurance_Master] as ins
                  ON i.Insurance_Guid = ins.Insurance_Id
                  WHERE i.[CoverCard_Number] IS NOT NULL
                  AND i.[Insurance_Valid_upto] > GETDATE()
                  AND p.[DateOfBirth] IS NOT NULL
                  AND (p.[LastName] LIKE '%test%' OR p.[FirstName] LIKE '%test%')
            ";
        }

        private static string CustomerWithRepetitionSql(int topRow)
        {
            return $@"
                SELECT TOP ({topRow}) p.[FirstName]  
                      ,p.[LastName]
                      ,MAX(p.[DateOfBirth]) AS [DateOfBirth]
                      ,MAX(p.[AddressId] ) AS [AddressId]
	                  ,MAX (p.[OmnichannelUUID]) AS [OmnichannelUUID]
                      ,ph.[PatientId]
                  FROM [ActivePos_read].[dbo].[IIIRepetition_Info] AS ri
                  INNER JOIN [ActivePos_read].[dbo].[IIIPrescription_hdr] AS ph
                  ON ri.[Parent_Prescription_No] = ph.[PrescriptionNo]
                  INNER JOIN (
                    SELECT p.[AddressId], p.[FirstName], p.[LastName], c.[PatientId], p.[DateOfBirth], p.[OmnichannelUUID]
                    FROM [ActivePos_read].[dbo].[IIIPerson] AS p
                    INNER JOIN [ActivePos_read].[dbo].[IIICustomerToPerson] AS ctp
                    ON p.[AddressId] = ctp.AddressId
                    INNER JOIN [ActivePos_read].[dbo].[IIICustomer] AS c
                    ON ctp.CustomerId = c.CustomerId
                    WHERE p.[DateOfBirth] IS NOT NULL
                  ) AS p
                  ON ph.PatientId = p.PatientId
                  WHERE ph.PatientId IN (
                    SELECT c.[PatientId]
                    FROM [ActivePos_read].[dbo].[IIIPerson] AS p
                    INNER JOIN [ActivePos_read].[dbo].[IIICustomerToPerson] AS ctp
                    ON p.[AddressId] = ctp.AddressId
                    INNER JOIN [ActivePos_read].[dbo].[IIICustomer] AS c
                    ON ctp.CustomerId = c.CustomerId
                    WHERE EXISTS (
                      SELECT 1
                      FROM (
                        SELECT p.[AddressId], MAX(c.[RowVersion]) AS [MaxRowVersion]
                        FROM [ActivePos_read].[dbo].[IIIPerson] AS p
                        INNER JOIN [ActivePos_read].[dbo].[IIICustomerToPerson] AS ctp
                        ON p.[AddressId] = ctp.AddressId
                        INNER JOIN [ActivePos_read].[dbo].[IIICustomer] AS c
                        ON ctp.CustomerId = c.CustomerId
                        WHERE p.Origin = 4
                        GROUP BY p.[AddressId], p.Origin
                      ) AS subquery
                      WHERE p.[AddressId] = subquery.[AddressId]
                      AND c.[RowVersion] = subquery.[MaxRowVersion]
                    )
                  )
                  AND ri.[IsOpen] = 1
                  AND p.[OmnichannelUUID] is not null
                  GROUP BY p.[FirstName] , p.[LastName], ph.[PatientId] 
                  ORDER BY p.[LastName]
            ";
        }

        //Method to get customer by AddressId
        private string CustomerByAddressIdSql(int topRow, int addressId)
        {
            return BaseSql(topRow) + $@"
            WHERE p.[AddressId] = {addressId}
            ";
        }

        // Method to get customers in an institution
        private string CustomerInInstitutionSql(int topRow)
        {
            return BaseSql(topRow) + @"
            WHERE c.[Institution] IS NOT NULL
            ";
        }

        // Method to get customers with a card
        private string CustomerCardSql(int topRow)
        {
            return BaseSql(topRow) + @"
            WHERE p.[CustomerCard] IS NOT NULL
            ";
        }

        // Method to get employee customers
        private string CustomerEmployeeSql(int topRow)
        {
            return BaseSql(topRow) + @"
            WHERE c.[EmployeeCardNumber] IS NOT NULL
            ";
        }

        // Method to get customers with AVH
        private string CustomerWithAVHSql(int topRow)
        {
            return BaseSql(topRow) + @"
            WHERE p.[AHVNumber] IS NOT NULL
            AND p.[AHVNumber] <> ''
            AND (p.[LastName] lIKE '%test%' OR p.[FirstName] lIKE '%test%')
            ";
        }

        #endregion

        #region SQLQueryRunner
        // Generic method to retrieve customer list from database
        private List<Customer> QueryCustomer(string connectionString, string sqlQuery)
        {
            var customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var customer = new Customer
                                    {
                                        FirstName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                        LastName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                        AddressId = reader.GetInt32(0),
                                        DateOfBirth = reader.IsDBNull(10) ? DateTime.MinValue : reader.GetDateTime(10),
                                        Sex = reader.GetInt32(9),
                                        Address1 = reader.IsDBNull(3) ? null : reader.GetString(3),
                                        Address2 = reader.IsDBNull(4) ? null : reader.GetString(4),
                                        ZipCode = reader.IsDBNull(5) ? null : reader.GetString(5),
                                        City = reader.IsDBNull(6) ? null : reader.GetString(6),
                                        CountryId = reader.GetInt32(8),
                                        OmnichannelUUID = reader.IsDBNull(16) ? null : reader.GetString(16),
                                        /*
                                        Language = reader.IsDBNull(10) ? null : reader.GetString(10),
                                        Dead = reader.GetBoolean(11), // Boolean
                                        AHVNumber = reader.IsDBNull(12) ? null : reader.GetString(12),
                                        CustomerCard = reader.IsDBNull(13) ? null : reader.GetString(13),
                                        Active = reader.GetBoolean(14), // Boolean
                                        AddressSupplement = reader.IsDBNull(16) ? null : reader.GetString(16),
                                        Height = reader.IsDBNull(17) ? null : reader.GetString(17),
                                        Weight = reader.IsDBNull(18) ? null : reader.GetString(18),
                                        EmployeeCardNumber = reader.IsDBNull(19) ? null : reader.GetString(19),
                                        Institution = reader.IsDBNull(20) ? null : reader.GetString(20)
                                        */
                                    };
                                    customers.Add(customer);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    var customerError = new Customer();
                    customerError.SqlError = ex.Message;
                    customers.Add(customerError);
                }
            }

            return customers;
        }

        // Generic method to retrieve customer list from database with insurance
        private List<Customer> QueryCustomersWithInsurance(string connectionString, string sqlQuery)
        {
            var customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var customer = new Customer
                                    {
                                        AddressId = reader.GetInt32(0),
                                        FirstName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                        LastName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                        CoverCardNumber = reader.IsDBNull(3) ? null : reader.GetString(3),
                                        InsuranceValidTo = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                                        DateOfBirth = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5),
                                        Address1 = reader.IsDBNull(6) ? null : reader.GetString(6),
                                        Address2 = reader.IsDBNull(7) ? null : reader.GetString(7),
                                        ZipCode = reader.IsDBNull(8) ? null : reader.GetString(8),
                                        City = reader.IsDBNull(9) ? null : reader.GetString(9),
                                        CountryId = reader.GetInt32(10),
                                        InsuranceName = reader.IsDBNull(11) ? null : reader.GetString(11),
                                    };
                                    customers.Add(customer);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return customers;
        }

        private List<Customer> QueryCustomersWithRepetition(string connectionString, string sqlQuery)
        {
            var customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var customer = new Customer
                                    {
                                        FirstName = reader.IsDBNull(0) ? null : reader.GetString(0),
                                        LastName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                        DateOfBirth = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                                        AddressId = reader.GetInt32(3),
                                        OmnichannelUUID = reader.IsDBNull(4) ? null : reader.GetString(4),
                                        PatientId = reader.GetGuid(5),
                                    };
                                    customers.Add(customer);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return customers;
        }

        #endregion

        #region PublicMethods
        // Instance method to retrieve customer list with valid insurance
        public List<Customer> GetCustomerWithValidInsurance(string connectionString, int topRow)
        {
            string query = CustomerWithValidInsuranceSql(topRow);
            return QueryCustomersWithInsurance(connectionString, query);
        }

        //Instance method to retrieve customer by AddressId
        public List<Customer> GetCustomerByAddressId(string connectionString, int topRow, int addressId)
        {
            string query = CustomerByAddressIdSql(topRow, addressId);
            return QueryCustomer(connectionString, query);
        }

        // Instance method to retrieve customer list in institution
        public List<Customer> GetCustomerInInstitution(string connectionString, int topRow)
        {
            string query = CustomerInInstitutionSql(topRow);
            return QueryCustomer(connectionString, query);
        }

        // Instance method to retrieve customer list with card
        public List<Customer> GetCustomerCard(string connectionString, int topRow)
        {
            string query = CustomerCardSql(topRow);
            return QueryCustomer(connectionString, query);
        }

        // Instance method to retrieve employee customer list
        public List<Customer> GetCustomerEmployee(string connectionString, int topRow)
        {
            string query = CustomerEmployeeSql(topRow);
            return QueryCustomer(connectionString, query);
        }

        // Instance method to retrieve customer list with AVH
        public List<Customer> GetCustomerWithAVH(string connectionString, int topRow)
        {
            string query = CustomerWithAVHSql(topRow);
            return QueryCustomer(connectionString, query);
        }

        public List<Customer> GetCustomerWithValidRepetition(string connectionString, int topRow)
        {
            string query = CustomerWithRepetitionSql(topRow);
            return QueryCustomersWithRepetition(connectionString, query);
        }

        // Method to display customer data
        public void DisplayCustomers(List<Customer> customers)
        {
            if (customers == null || customers.Count == 0)
            {
                Console.WriteLine("No customers to display.");
                return;
            }

            foreach (var customer in customers)
            {
                string formattedFirstName = !string.IsNullOrEmpty(customer.FirstName)
                    ? char.ToUpper(customer.FirstName[0]) + customer.FirstName.Substring(1).ToLower()
                    : customer.FirstName;

                Console.WriteLine($"{formattedFirstName} {customer.LastName.ToUpper()}");
            }
        }

        #endregion
    }
}
