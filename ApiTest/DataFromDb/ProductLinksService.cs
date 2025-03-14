using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFromDb
{
    public class ProductLinksService
    {
        public static string GetSqlProduct(
            string searchTerm = null,
            string gtinEanCode = null,
            string pharmacode = null,
            int linkType = 5, // Defaut replacement
            bool needGtinEanCode = false,
            Language language = Language.French,
            int nbRow = 100) 
        {
            /*
            Substitution = 1,
            ItemWithBetterMargin = 2,
            AssociateItem = 3,
            ItemPromotion = 4,
            Replacement = 5,
            ItemToPrintOnTicket = 6,
            RemarkToPrintOnTicket = 7,
            MedicalSet = 8,
            Migel/Lima = 9,
            Discount = 10,
            Biosimilar = 12
            */
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
                    SELECT TOP ({nbRow}) il.[ITL_item] AS OriginalItemId --int, not null
	                ,itk.ITK_key AS Original{label} -- varchar(30), null
	                ,it.ITTX_description  AS OriginalDescription -- varchar(255), not null
                    ,il.[ITL_linked_item] AS ChainedItemId --int, not null
	                ,itk2.ITK_key AS Chained{label} -- varchar(30), null
	                ,it2.ITTX_description  AS LinkedDescription -- varchar(255), not null
                    ,il.[ITL_type] -- smallint, not null
                    ,il.[ITL_start_date] --DateTime, null
                    ,il.[ITL_end_date] --DateTime, null
                    ,il.[ITL_active] -- Boolean, not null, default 1
                  FROM [Arizona].[dbo].[Item_link] AS il
                  JOIN item_text AS it
                    ON il.[ITL_item] = it.ITTX_item 
                  JOIN [item_text] AS it2 -- for chain part
	                ON il.[ITL_linked_item] = it2.ITTX_item -- for chain part
                  JOIN item_key AS itk
                    ON itk.ITK_item =  il.[ITL_item]
                  JOIN item_key AS itk2
                    ON itk2.ITK_item = il.[ITL_linked_item]
                  WHERE il.[ITL_active] = 1
                  AND it.ITTX_language = {(int)language}
                  AND it2.ITTX_language = {(int)language}
                  AND il.[ITL_type] = {linkType} -- see above for link type
                  AND itk.ITK_type = {itemType} -- 5=EanCode/GTIN 1=Pharmacode
                  AND itk2.ITK_type = {itemType} -- 5=EanCode/GTIN 1=Pharmacode
                ";

            if (searchTerm != null)
                query += $"AND it.ITTX_description LIKE '%{searchTerm}%'";

            if (gtinEanCode != null)
                query += $" AND itk.ITK_key LIKE '%{gtinEanCode}%'";

            if (pharmacode != null)
                query += $" AND itk.ITK_key LIKE '%{pharmacode}%'";



            query += " order by il.[ITL_item]";

            return query;
        }

        public static List<ProductLinks> GetProducts(
            string connectionString,
            string searchTerm = null,
            string gtinEanCode = null,
            string pharmacode = null,
            int linkType = 5, // Defaut replacement
            bool needGtinEanCode = false,
            Language language = Language.French,
            int nbRow = 100) 
        {
            string query = GetSqlProduct(searchTerm, gtinEanCode, pharmacode, linkType, needGtinEanCode, language, nbRow);

            List<ProductLinks> products = new List<ProductLinks>();
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
                                    if (gtinEanCode != null || needGtinEanCode)
                                    {
                                        var product = new ProductLinks
                                        {
                                            OriginalItemId = reader.GetInt32(0).ToString(),
                                            OriginalGtinEanCode = reader.IsDBNull(1) ? null : reader.GetString(1),
                                            OriginalDescription = reader.IsDBNull(2) ? null : reader.GetString(2),
                                            LinkedItemId = reader.GetInt32(3).ToString(),
                                            LinkedGtinEanCode = reader.IsDBNull(4) ? null : reader.GetString(4),
                                            LinkedDescription = reader.IsDBNull(5) ? null : reader.GetString(5),
                                            LinkType = reader.GetInt16(6),
                                            LinkStartDate = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7),
                                            LinkEndDate = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8),
                                            IsActive = reader.IsDBNull(9) ? true : reader.GetBoolean(9)
                                        };
                                        products.Add(product);
                                    }
                                    else
                                    {
                                        var product = new ProductLinks
                                        {
                                            OriginalItemId = reader.GetInt32(0).ToString(),
                                            OriginalPharmacode = reader.IsDBNull(1) ? null : reader.GetString(1),
                                            OriginalDescription = reader.IsDBNull(2) ? null : reader.GetString(2),
                                            LinkedItemId = reader.GetInt32(3).ToString(),
                                            LinkedPharmacode = reader.IsDBNull(4) ? null : reader.GetString(4),
                                            LinkedDescription = reader.IsDBNull(5) ? null : reader.GetString(5),
                                            LinkType = reader.GetInt16(6),
                                            LinkStartDate = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7),
                                            LinkEndDate = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8),
                                            IsActive = reader.IsDBNull(9) ? true : reader.GetBoolean(9)
                                        };
                                        products.Add(product);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    products.Add(new ProductLinks { Error = ex.Message });
                }
            }

            return products;
        }
    }
}
