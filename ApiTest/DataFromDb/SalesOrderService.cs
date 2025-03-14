using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataFromDb
{
    public static class SalesOrderService
    {
        public static string GetSqlSalesOrder(
            string externalReferenceNumber = null,
            string userTrigram = null,
            string patientName = null,
            string paymentType = null,
            string onlineOrderId = null,
            int? deliveryMode = null,
            Guid? onlineOrderFulfillmentId = null,
            int? onlineSaleStatus = null,
            int? customerId = null,
            int nbRow = 100)
        {
            string sqlQuery = $@"
                SELECT TOP ({nbRow}) [SalesOrderId]
                      ,[Date]
                      ,[UserId]
                      ,[SignedByUserId]
                      ,[OfflineSalesOrderNumber]
                      ,[OfflineBillNumber]
                      ,[Duty]
                      ,[PaymentType]
                      ,[PosId]
                      ,[CustomerRemark]
                      ,[StockMovement]
                      ,[TransactionType]
                      ,[PendingType]
                      ,[Status]
                      ,[ModifiedDate]
                      ,[PhysicianName]
                      ,[UserTrigram]
                      ,[TotalAmount]
                      ,[PatientName]
                      ,[PickUpNumber]
                      ,[CustomerId]
                      ,[ExternalReferenceNumber]
                      ,[EmployeeCardNumber]
                      ,[DeliveryMode]
                      ,[OnlineSaleStatus]
                      ,[OnlineOrderId]
                      ,[OnlineOrderFulfillmentId]
                  FROM [ActivePos_server].[dbo].[SalesOrder]
                  WHERE [SalesOrderId] IS NOT NULL
                ";

            if (externalReferenceNumber != null)
                sqlQuery += $" AND [ExternalReferenceNumber] LIKE '%{externalReferenceNumber}%'";

            if (userTrigram != null)
                sqlQuery += $" AND [UserTrigram] LIKE '%{userTrigram}%'";

            if (patientName != null)
                sqlQuery += $" AND [PatientName] LIKE '%{patientName}%'";

            if (paymentType != null) // could be C (cash) or I (insurance)
                sqlQuery += $" AND [PaymentType] LIKE '%{paymentType}%'";

            if (onlineOrderId != null)
                sqlQuery += $" AND [OnlineOrderId] LIKE '%{onlineOrderId}%'";

            if (deliveryMode != null) // could be 0(not delivered) or 1 (delivered)
                sqlQuery += $" AND [DeliveryMode] = {deliveryMode}";

            if (onlineOrderFulfillmentId != null)
                sqlQuery += $" AND [OnlineOrderFulfillmentId] = '{onlineOrderFulfillmentId}'";

            if (onlineSaleStatus != null) // could be 8,5,4,2,0
                sqlQuery += $" AND [OnlineSaleStatus] = {onlineSaleStatus}";

            if (customerId != null)
                sqlQuery += $" AND [CustomerId] LIKE '%{customerId}%'";

            sqlQuery += " ORDER BY [Date] DESC";

            return sqlQuery;
        }

        public static List<SalesOrder> GetSalesOrder(
             string connectionString,
             string externalReferenceNumber = null,
             string userTrigram = null,
             string patientName = null,
             string paymentType = null,
             string onlineOrderId = null,
             int? deliveryMode = null,
             Guid? onlineOrderFulfillmentId = null,
             int? onlineSaleStatus = null,
             int? customerId = null,
             int nbRow = 100)
        {
            var salesOrders = new List<SalesOrder>();

            try
            {
                string sqlQuery = GetSqlSalesOrder(
                    externalReferenceNumber, userTrigram, patientName, paymentType,
                    onlineOrderId, deliveryMode, onlineOrderFulfillmentId,
                    onlineSaleStatus, customerId, nbRow);

                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var salesOrder = new SalesOrder
                                {
                                    SalesOrderId = reader.GetGuid(reader.GetOrdinal("SalesOrderId")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    SignedByUserId = reader.IsDBNull(reader.GetOrdinal("SignedByUserId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SignedByUserId")),
                                    OfflineSalesOrderNumber = reader.IsDBNull(reader.GetOrdinal("OfflineSalesOrderNumber")) ? null : reader.GetString(reader.GetOrdinal("OfflineSalesOrderNumber")),
                                    OfflineBillNumber = reader.IsDBNull(reader.GetOrdinal("OfflineBillNumber")) ? null : reader.GetString(reader.GetOrdinal("OfflineBillNumber")),
                                    Duty = reader.GetInt32(reader.GetOrdinal("Duty")),
                                    PaymentType = reader.IsDBNull(reader.GetOrdinal("PaymentType")) ? (char?)null : reader.GetString(reader.GetOrdinal("PaymentType"))[0],
                                    PosId = reader.IsDBNull(reader.GetOrdinal("PosId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("PosId")),
                                    CustomerRemark = reader.IsDBNull(reader.GetOrdinal("CustomerRemark")) ? null : reader.GetString(reader.GetOrdinal("CustomerRemark")),
                                    StockMovement = reader.GetBoolean(reader.GetOrdinal("StockMovement")),
                                    TransactionType = reader.GetInt32(reader.GetOrdinal("TransactionType")),
                                    PendingType = reader.GetInt32(reader.GetOrdinal("PendingType")),
                                    Status = reader.GetInt32(reader.GetOrdinal("Status")),
                                    ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate")),
                                    PhysicianName = reader.IsDBNull(reader.GetOrdinal("PhysicianName")) ? null : reader.GetString(reader.GetOrdinal("PhysicianName")),
                                    UserTrigram = reader.IsDBNull(reader.GetOrdinal("UserTrigram")) ? null : reader.GetString(reader.GetOrdinal("UserTrigram")),
                                    TotalAmount = reader.IsDBNull(reader.GetOrdinal("TotalAmount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                    PatientName = reader.IsDBNull(reader.GetOrdinal("PatientName")) ? null : reader.GetString(reader.GetOrdinal("PatientName")),
                                    PickUpNumber = reader.IsDBNull(reader.GetOrdinal("PickUpNumber")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("PickUpNumber")),
                                    CustomerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CustomerId")),
                                    ExternalReferenceNumber = reader.IsDBNull(reader.GetOrdinal("ExternalReferenceNumber")) ? null : reader.GetString(reader.GetOrdinal("ExternalReferenceNumber")),
                                    EmployeeCardNumber = reader.IsDBNull(reader.GetOrdinal("EmployeeCardNumber")) ? null : reader.GetString(reader.GetOrdinal("EmployeeCardNumber")),
                                    DeliveryMode = reader.GetInt32(reader.GetOrdinal("DeliveryMode")),
                                    OnlineSaleStatus = reader.GetByte(reader.GetOrdinal("OnlineSaleStatus")),
                                    OnlineOrderId = reader.IsDBNull(reader.GetOrdinal("OnlineOrderId")) ? null : reader.GetString(reader.GetOrdinal("OnlineOrderId")),
                                    OnlineOrderFulfillmentId = reader.IsDBNull(reader.GetOrdinal("OnlineOrderFulfillmentId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("OnlineOrderFulfillmentId"))
                                };
                                salesOrders.Add(salesOrder);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Add a SalesOrder instance with the error message
                salesOrders.Add(new SalesOrder
                {
                    Error = ex.Message
                });
            }

            return salesOrders;
        }
    }
}