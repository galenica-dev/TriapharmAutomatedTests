using System;

namespace DataFromDb
{
    public class Repetition
    {
        public string RepetitionNo { get; set; }
        public string ParentPrescriptionNo { get; set; }
        public string PatientId { get; set; }
        public string PhysicianId { get; set; }
        public string ParentSalesOrderNo { get; set; }
        public string RepetitionType { get; set; }
        public DateTime? LimitDate { get; set; }
        public DateTime? DefaultEndDateRepetition { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? RemainingQuantity { get; set; }
        public string PrescriptionNo { get; set; }
        public string SalesOrderId { get; set; }
        public string SalesOrderDetailId { get; set; }
        public string ItemDescription { get; set; }
        public string ItemId { get; set; }
        public string Error { get; set; }
    }
}
