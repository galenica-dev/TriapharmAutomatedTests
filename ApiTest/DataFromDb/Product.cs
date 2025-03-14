using System;
using System.Text.Json.Serialization;

namespace DataFromDb
{
    public class Product
    {
        public string ItemId { get; set; }
        public string ItemCode { get; set; }
        public bool IsPrescriptionMandatory { get; set; }
        public string InsuranceCode { get; set; }
        public string ProductNumber { get; set; }
        public short? MonthValidity { get; set; }
        public bool IsNarcotic { get; set; }
        public string NarcoticCode { get; set; }
        public bool IsRequiringFridge { get; set; }
        public string TemperatureStorage { get; set; }
        public Biosimilar BioSimilar { get; set; }
        // Computed property for JSON serialization
        public string BioSimilarString => BioSimilar.ToString();
        public bool IsToxic { get; set; }
        public string ToxicClass { get; set; }
        public DateTime? WithdrawlDate { get; set; }
        public int ItemTextId { get; set; }
        public Language Language { get; set; }
        public string Description { get; set; }
        public string UsageDescription { get; set; }
        public string GtinEanCode { get; set; }
        public string Pharmacode { get; set; }
        public int? StockQuantity { get; set; }
        public int SupplierId { get; set; }
        public Decimal MinimumQuantiy { get; set; }
        public Decimal MaximumQuantity { get; set; }
        public string Error { get; set; }
        public int? SupplierAddressId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public DateTime? PriceStartDate { get; set; }
        public DateTime? PriceEndDate { get; set; }
        public Decimal PricePerUnitTaxIncluded { get; set; }
        public Decimal QuantityInStock { get; set; }

        // Override ToString for debugging purposes
        public override string ToString()
        {
            return $@"
                ItemId: {ItemId},
                ItemCode: {ItemCode},
                IsPrescriptionMandatory: {IsPrescriptionMandatory},
                InsuranceCode: {InsuranceCode},
                ProductNumber: {ProductNumber},
                MonthValidity: {MonthValidity?.ToString() ?? "N/A"},
                IsNarcotic: {IsNarcotic},
                NarcoticCode: {NarcoticCode},
                IsRequiringFridge: {IsRequiringFridge},
                TemperatureStorage: {TemperatureStorage},
                BioSimilar: {BioSimilar},
                IsToxic: {IsToxic},
                ToxicClass: {ToxicClass},
                WithdrawlDate: {WithdrawlDate?.ToString("yyyy-MM-dd") ?? "N/A"},
                ItemTextId: {ItemTextId},
                Language: {Language},
                Description: {Description},
                UsageDescription: {UsageDescription},
                EanCode: {GtinEanCode},
                StockQuantity: {StockQuantity},
                SupplierId: {SupplierId},
                MinimumQuantiy: {MinimumQuantiy},
                MaximumQuantity: {MaximumQuantity},
                Error: {Error}";

        }
    }
}
