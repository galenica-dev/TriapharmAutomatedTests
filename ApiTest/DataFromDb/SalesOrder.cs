using System;
using System.Text;

namespace DataFromDb
{
    public class SalesOrder
    {
        public Guid SalesOrderId { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int? SignedByUserId { get; set; }
        public string OfflineSalesOrderNumber { get; set; }
        public string OfflineBillNumber { get; set; }
        public int Duty { get; set; }
        public char? PaymentType { get; set; }
        public int? PosId { get; set; } = 0; // Default value
        public string CustomerRemark { get; set; }
        public bool StockMovement { get; set; } = true; // Default value
        public int TransactionType { get; set; }
        public int PendingType { get; set; } = 1; // Default value
        public int Status { get; set; } = 1; // Default value
        public DateTime ModifiedDate { get; set; } = DateTime.Now; // Default value
        public string PhysicianName { get; set; }
        public string UserTrigram { get; set; }
        public decimal? TotalAmount { get; set; }
        public string PatientName { get; set; }
        public Guid? PickUpNumber { get; set; }
        public int? CustomerId { get; set; }
        public string ExternalReferenceNumber { get; set; }
        public string EmployeeCardNumber { get; set; }
        public int DeliveryMode { get; set; } = 0; // Default value
        public byte OnlineSaleStatus { get; set; } = 0; // Default value
        public string OnlineOrderId { get; set; }
        public Guid? OnlineOrderFulfillmentId { get; set; }
        public string Error { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"SalesOrderId: {SalesOrderId}");
            sb.AppendLine($"Date: {Date}");
            sb.AppendLine($"UserId: {UserId}");
            sb.AppendLine($"SignedByUserId: {SignedByUserId}");
            sb.AppendLine($"OfflineSalesOrderNumber: {OfflineSalesOrderNumber}");
            sb.AppendLine($"OfflineBillNumber: {OfflineBillNumber}");
            sb.AppendLine($"Duty: {Duty}");
            sb.AppendLine($"PaymentType: {PaymentType}");
            sb.AppendLine($"PosId: {PosId}");
            sb.AppendLine($"CustomerRemark: {CustomerRemark}");
            sb.AppendLine($"StockMovement: {StockMovement}");
            sb.AppendLine($"TransactionType: {TransactionType}");
            sb.AppendLine($"PendingType: {PendingType}");
            sb.AppendLine($"Status: {Status}");
            sb.AppendLine($"ModifiedDate: {ModifiedDate}");
            sb.AppendLine($"PhysicianName: {PhysicianName}");
            sb.AppendLine($"UserTrigram: {UserTrigram}");
            sb.AppendLine($"TotalAmount: {TotalAmount}");
            sb.AppendLine($"PatientName: {PatientName}");
            sb.AppendLine($"PickUpNumber: {PickUpNumber}");
            sb.AppendLine($"CustomerId: {CustomerId}");
            sb.AppendLine($"ExternalReferenceNumber: {ExternalReferenceNumber}");
            sb.AppendLine($"EmployeeCardNumber: {EmployeeCardNumber}");
            sb.AppendLine($"DeliveryMode: {DeliveryMode}");
            sb.AppendLine($"OnlineSaleStatus: {OnlineSaleStatus}");
            sb.AppendLine($"OnlineOrderId: {OnlineOrderId}");
            sb.AppendLine($"OnlineOrderFulfillmentId: {OnlineOrderFulfillmentId}");
            sb.AppendLine($"Error: {Error}");
            return sb.ToString();
        }
    }
}
