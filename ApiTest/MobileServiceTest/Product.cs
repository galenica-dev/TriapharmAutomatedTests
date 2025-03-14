namespace MobileServiceTest
{
    public class Product
    {
        public string EanCode { get; set; }
        public string Description { get; set; }
        public string PharmaCode { get; set; }
        public int QuantityOnStock { get; set; }
        public bool InStock { get; set; }
        public decimal Price { get; set; }

        // Constructor
        public Product(string eanCode, string description, string pharmaCode, int quantityOnStock, bool inStock, decimal price)
        {
            EanCode = eanCode;
            Description = description;
            PharmaCode = pharmaCode;
            QuantityOnStock = quantityOnStock;
            InStock = inStock;
            Price = price;
        }

        // ToString method to display Product information
        public override string ToString()
        {
            return $"{EanCode} - {Description}";
        }
    }
}
