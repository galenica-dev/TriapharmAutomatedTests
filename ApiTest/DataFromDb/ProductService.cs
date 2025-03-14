using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DataFromDb
{
    public static class ProductService
    {
        // Static method to fetch products from the database
        public static List<(string ItkKey, decimal FpPubPrice, decimal FpMmrPrice)> GetLimaProducts(string connectionString)
        {
            // Define the SQL query
            string query = @"
            SELECT TOP 10 itk_key, fp_FPUB.FP_tax_incl_price_per_unit, fp_mmr.FP_tax_incl_price_per_unit  
            FROM Arizona.dbo.item_key
            JOIN Arizona.dbo.Fixed_price fp_mmr
            ON itk_item = fp_mmr.fp_item
            AND itk_type = 1
            JOIN arizona.dbo.Price_code prc_mmr
            ON fp_mmr.FP_price_code = prc_mmr.Price_code_ID
            JOIN Arizona.dbo.Fixed_price fp_FPUB
            ON itk_item = fp_FPUB.fp_item
            AND itk_type = 1
            JOIN arizona.dbo.Price_code prc_FPUB
            ON fp_FPUB.FP_price_code = prc_FPUB.Price_code_ID
            WHERE fp_mmr.FP_start_date < GETDATE()
            AND prc_mmr.prc_code = 'mmr'
            AND (fp_mmr.FP_end_date IS NULL OR fp_mmr.FP_end_date > GETDATE())
            AND fp_FPUB.FP_start_date < GETDATE()
            AND prc_FPUB.prc_code = 'fpub'
            AND (fp_FPUB.FP_end_date IS NULL OR fp_FPUB.FP_end_date > GETDATE())
            AND fp_FPUB.FP_tax_incl_price_per_unit > fp_mmr.FP_tax_incl_price_per_unit
        ";

            var products = new List<(string ItkKey, decimal FpPubPrice, decimal FpMmrPrice)>();

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
                                    // Add each product as a tuple to the list
                                    products.Add((
                                        reader.GetString(0),     // itk_key
                                        reader.GetDecimal(1),    // fp_FPUB.FP_tax_incl_price_per_unit
                                        reader.GetDecimal(2)     // fp_mmr.FP_tax_incl_price_per_unit
                                    ));
                                }
                            }
                            else
                            {
                                return null;  // Return null if no rows are found
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that may have occurred
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return products.Count > 0 ? products : null;
        }

        // Static method to display the products
        public static void DisplayProducts(List<(string ItkKey, decimal FpPubPrice, decimal FpMmrPrice)> products)
        {
            if (products == null)
            {
                Console.WriteLine("No products found.");
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine($"{product.ItkKey}: FP PUB = {product.FpPubPrice}, FP MMR = {product.FpMmrPrice}");
            }
        }

        public static string GetSqlProductInfo(
            string searchTerm = null,
            bool isRequiringFridge = false,
            bool isNarcotic = false,
            bool isRequiringSubscription = false,
            bool hasMinMaxQty = false,
            Biosimilar biosimilar = Biosimilar.Empty,
            string gtinEanCode = null,
            string pharmacode = null,
            bool needGtinEanCode = false, 
            Language language = Language.French,
            decimal? priceEqBigger = null,
            decimal? priceEqSmaller = null,
            int? quantityStockEqBigger = null,
            int? quantityStockEqSmaller = null,
            string supplierName = null,
            string supplierCode = null,
            int? itemId = null,
            int nbRow = 100)
        {
            int itemType = 5;
            string label = "gtinEanCode";
            //force to only 1 of gtinEanCode or pharmacode
            if (gtinEanCode != null)
                pharmacode = null;
            else if (needGtinEanCode) 
            { 
                itemType = 5;
                label = "gtinEanCode";
            }
            else
            {
                itemType = 1;
                label = "pharmacode";
            }

            string query = $@"
                SELECT TOP {nbRow} phi.PHIT_item AS ItemId, -- int, not null, same as fp.FP_item 
                    phi.PHIT_code AS ItemCode, -- varchar(15), null
                    itk.Item_key_ID AS ItemKeyId, -- int, not null
                    itk.ITK_key AS {label}, -- varchar(30), null
                    phi.PHIT_Product_number AS ProductNumber, -- int, null
                    it.Item_text_ID AS ItemTextId, -- int, not null
                    ioc.SupplierId AS SupplierId, -- int, not null
	                a.Address_ID AS SupplierAddressId, -- int, not null
	                adk.ADK_key AS SupplierCode, -- varchar(15), null
                    phi.PHIT_prescription_mandatory AS IsPrescriptionMandatory, -- Boolean, not null
                    phi.PHIT_insurance_code AS InsuranceCode, -- smallint, null
                    phi.PHIT_expiry AS MonthValidity, -- smallint, null
                    phi.PHIT_narcotic AS IsNarcotic , -- varchar(1), N=False, Y=True, null
                    phi.PHIT_narcotic_code AS NarcoticCode, -- varchar(1), null
                    phi.PHIT_refrigeration AS IsRequiringFridge, -- int, 1=True, null or other value = false, null
                    phi.PHIT_temperature AS TemperatureStorage, -- varchar(10), null
                    phi.PHIT_BioSimilar AS BioSimilarCode, -- varchar(1), null
                    phi.PHIT_toxic AS IsToxic, -- bit, null
                    phi.PHIT_toxicity_class AS ToxicClass, -- varchar(4), null
                    phi.PHIT_withdrawal_date AS WithdrawalDate, -- datetime, null
                    it.ITTX_language AS [Language], -- int, not null
                    it.ITTX_description AS [Description], -- varchar(255), not null 
                    it.ITTX_usage_description AS UsageDescription, -- varchar(60), not null
	                a.[AD_name] AS SupplierName, -- varchar(15), null
                    ioc.MinimumQuantity AS MinimumQuantity, -- decimal(11, 4), null
                    ioc.MaximumQuantity AS MaximumQuantity, -- decimal(11, 4), null
                    fp.FP_start_date AS PriceStartDate, -- datetime, null
                    fp.FP_end_date AS PriceEndDate, -- datetime, null
                    fp.FP_tax_incl_price_per_unit AS PricePerUnitTaxIncluded, -- decimal(14, 2), null
                    dbo.aps_fn_Item_Stock_Qty(phi.PHIT_item, (SELECT TOP (1) [Inventory_type_ID] FROM [Arizona].[dbo].[Inventory_type]), NULL, NULL) AS QuantityInStock -- Calculate stock -- decimal(11, 4), null
                FROM PH_item AS phi
                    JOIN item_text AS it
                        ON phi.PHIT_item = it.ITTX_item
                    JOIN item_key AS itk
                        ON itk.ITK_item = phi.PHIT_item
                    JOIN [wkl].[ItemOrderCondition] AS ioc
                        ON ioc.ItemId = phi.PHIT_item
                    JOIN fixed_price AS fp
                        ON itk.ITK_item = fp.FP_item
                    JOIN [Arizona].[dbo].[Supplier] AS s
	                    ON s.[Supplier_ID] = ioc.SupplierId
                    JOIN [Arizona].[dbo].[Address] AS a
                       ON a.[Address_ID] = s.[SUPP_address]
                    JOIN address_key AS adk
                       ON a.Address_ID = adk.ADK_address
                WHERE phi.PHIT_item IS NOT NULL 
                    AND itk.ITK_type = {itemType} 
                    AND it.ITTX_language = {(int)language} 
                    ";

            if (searchTerm != null)
                query += $" AND it.ITTX_description LIKE '%{searchTerm}%' ";

            if (isRequiringFridge)
                query += " AND (phi.PHIT_refrigeration IS NOT NULL AND phi.PHIT_refrigeration > 0)  ";

            if (isNarcotic)
                query += " AND phi.PHIT_narcotic = 'Y' ";

            if (isRequiringSubscription) // Add the new condition
                query += " AND phi.PHIT_prescription_mandatory = 1 ";
            else
                query += " AND phi.PHIT_prescription_mandatory = 0 ";

            if (hasMinMaxQty)
                query += " AND (ioc.MinimumQuantity > 0 OR ioc.MaximumQuantity > 0) ";

            if (biosimilar != Biosimilar.Empty)
                query += $" AND phi.PHIT_BioSimilar = '{biosimilar}' ";

            if (gtinEanCode != null)
                query += $" AND itk.ITK_key LIKE '%{gtinEanCode}%'  ";

            if (pharmacode != null)
                query += $" AND itk.ITK_key LIKE '%{pharmacode}%' ";

            if (priceEqBigger > 0)
                query += $" AND fp.FP_tax_incl_price_per_unit >= {priceEqBigger}  ";

            if (priceEqSmaller > 0)
                query += $" AND fp.FP_tax_incl_price_per_unit <= {priceEqSmaller}  ";

            if (quantityStockEqBigger > 0)
                query += $" AND dbo.aps_fn_Item_Stock_Qty(phi.PHIT_item, (SELECT TOP (1) [Inventory_type_ID] FROM [Arizona].[dbo].[Inventory_type]), NULL, NULL) >= {quantityStockEqBigger}  ";

            if (quantityStockEqSmaller > 0)
                query += $" AND dbo.aps_fn_Item_Stock_Qty(phi.PHIT_item, (SELECT TOP (1) [Inventory_type_ID] FROM [Arizona].[dbo].[Inventory_type]), NULL, NULL) <= {quantityStockEqSmaller}  ";

            if (supplierName != null)
                query += $" AND a.[AD_name] LIKE '%{supplierName}%' ";

            if (supplierCode != null)
                query += $" AND adk.ADK_key LIKE '%{supplierCode}%' ";

            if (itemId != null)
                query += $" AND phi.PHIT_item = {itemId} ";

            query += $@" 
                AND phi.PHIT_withdrawal_date IS NULL 
                    AND (fp.FP_end_date >= GETDATE() OR fp.FP_end_date IS NULL) 
                    AND fp.FP_tax_incl_price_per_unit IS NOT NULL 
                    AND adk.ADK_type = 1 
                ORDER BY itk.ITK_key ";

            return query;
        }

        public static List<Product> GetProducts(
            string connectionString,
            string searchTerm = null,
            bool isRequiringFridge = false,
            bool isNarcotic = false,
            bool isRequiringSubscription = false,
            bool hasMinMaxQty = false,
            Biosimilar biosimilar = Biosimilar.Empty,
            string gtinEanCode = null,
            string pharmacode = null,
            bool needGtinEanCode = false,
            Language language = Language.French,
            decimal? priceEqBigger = null,
            decimal? priceEqSmaller = null,
            int? quantityStockEqBigger = null,
            int? quantityStockEqSmaller = null,
            string supplierName = null,
            string supplierCode = null,
            int? itemId = null,
            int nbRow = 100)
        {
            string query = GetSqlProductInfo(searchTerm, isRequiringFridge, isNarcotic, isRequiringSubscription, hasMinMaxQty, biosimilar, gtinEanCode, pharmacode, needGtinEanCode,
                language, priceEqBigger, priceEqSmaller, quantityStockEqBigger, quantityStockEqSmaller, supplierName, supplierCode, itemId, nbRow);

            var products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var product = CreateProductFromReader(reader, gtinEanCode != null || needGtinEanCode);
                                    products.Add(product);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    products.Add(new Product { Error = ex.Message });
                }
            }

            return products;
        }

        private static Product CreateProductFromReader(SqlDataReader reader, bool useGtinEanCode)
        {
            return new Product
            {
                ItemId = reader.GetInt32(0).ToString(),
                ItemCode = reader.IsDBNull(1) ? null : reader.GetString(1),
                GtinEanCode = useGtinEanCode ? (reader.IsDBNull(3) ? null : reader.GetString(3)) : null,
                Pharmacode = !useGtinEanCode ? (reader.IsDBNull(3) ? null : reader.GetString(3)) : null,
                ProductNumber = reader.IsDBNull(4) ? null : reader.GetInt32(4).ToString(),
                ItemTextId = reader.GetInt32(5),
                SupplierId = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                SupplierAddressId = reader.GetInt32(7),
                SupplierCode = reader.IsDBNull(8) ? null : reader.GetString(8),
                IsPrescriptionMandatory = reader.GetBoolean(9),
                InsuranceCode = reader.IsDBNull(10) ? null : reader.GetInt16(10).ToString(),
                MonthValidity = reader.IsDBNull(11) ? (short?)null : reader.GetInt16(11),
                IsNarcotic = reader.IsDBNull(12) ? false : reader.GetString(12) == "Y",
                NarcoticCode = reader.IsDBNull(13) ? null : reader.GetString(13),
                IsRequiringFridge = !reader.IsDBNull(14) && reader.GetInt32(14) > 0,
                TemperatureStorage = reader.IsDBNull(15) ? null : reader.GetString(15),
                BioSimilar = reader.IsDBNull(16) ? Biosimilar.Empty : (Biosimilar)Enum.Parse(typeof(Biosimilar), reader.GetString(16)),
                IsToxic = reader.IsDBNull(17) ? false : reader.GetBoolean(17),
                ToxicClass = reader.IsDBNull(18) ? null : reader.GetString(18),
                WithdrawlDate = reader.IsDBNull(19) ? (DateTime?)null : reader.GetDateTime(19),
                Language = (Language)reader.GetInt32(20),
                Description = reader.IsDBNull(21) ? null : reader.GetString(21),
                UsageDescription = reader.IsDBNull(22) ? null : reader.GetString(23),
                SupplierName = reader.IsDBNull(23) ? null : reader.GetString(23),
                MinimumQuantiy = reader.IsDBNull(24) ? 0 : reader.GetDecimal(24),
                MaximumQuantity = reader.IsDBNull(25) ? 0 : reader.GetDecimal(25),
                PriceStartDate = reader.IsDBNull(26) ? (DateTime?)null : reader.GetDateTime(26),
                PriceEndDate = reader.IsDBNull(27) ? (DateTime?)null : reader.GetDateTime(27),
                PricePerUnitTaxIncluded = reader.GetDecimal(28),
                QuantityInStock = reader.IsDBNull(29) ? 0 : reader.GetDecimal(29)
            };
        }
    }
}
