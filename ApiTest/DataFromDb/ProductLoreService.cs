using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFromDb
{
    public class ProductLoreService
    {
        public static string GetSqlProduct(
            string searchTerm = null,
            string gtinEanCode = null,
            string pharmacode = null,
            bool needGtinEanCode = false,
            Language language = Language.French,
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
                select TOP {nbRow}
	            itk_key AS {label}, -- varchar(30), null
                ittx_description AS [Description] -- varchar(255), not null 

                from Bmc_application_key 
                  join Bmc_application_default
                    on BAPD_bmc_application_key = bmc_application_key_id
                  join criteria
                    on criteria_id = bapd_value
                  join criteria_type
                    on criteria_type_id = cr_criteria_type
 
                  join item_criteria
                    on itcr_criteria = criteria_id
 
                  join item_key
                    on itk_item = ITCR_item
                   and itk_type = {itemType}
 
                  join item_text
                    on ittx_item = itk_item
                   and ittx_language = {(int)language}
 
                where BAPK_key = 'cvPhLoreAutomaticCriteria'";

            if (searchTerm != null)
                query +=$" AND ittx_description LIKE '%{searchTerm}%'";

            if (gtinEanCode != null)
                query += $" AND itk_key LIKE '%{gtinEanCode}%'";

            if (pharmacode != null)
                query += $" AND itk_key LIKE '%{pharmacode}%'";

            query += " order by 1";
            return query;   
        }

        public static List<ProductLore> GetProducts(
            string connectionString,
            string searchTerm = null,
            string gtinEanCode = null,
            string pharmacode = null,
            bool needGtinEanCode = false,
            Language language = Language.French,
            int nbRow = 100) 
        {
            string query = GetSqlProduct(searchTerm, gtinEanCode, pharmacode, needGtinEanCode, language, nbRow);

            List<ProductLore> products = new List<ProductLore>();
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
                                        var product = new ProductLore
                                        {
                                            GtinEanCode = reader.IsDBNull(0) ? null : reader.GetString(0),
                                            Description = reader.IsDBNull(1) ? null : reader.GetString(1)
                                        };
                                        products.Add(product);
                                    }
                                    else
                                    {
                                        var product = new ProductLore
                                        {
                                            Pharmacode = reader.IsDBNull(0) ? null : reader.GetString(0),
                                            Description = reader.IsDBNull(1) ? null : reader.GetString(1)
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
                    products.Add(new ProductLore { Error = ex.Message });
                }
            }

            return products;
        }
    }
}
