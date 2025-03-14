using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Hci.WcfToolkit.Client;
using MobileServiceTest.MobileServiceReference;

namespace MobileServiceTest
{
    public class ProductService
    {
        private string _urlService = "https://localhost:33365/ActivePharmacy/MobileService/MobileService.svc";

        // Method to search for articles based on the medicine name
        public List<Product> SearchArticles(string medicineName)
        {
            Uri uri = new Uri(_urlService);
            ChannelFactory<MobileServiceChannel> factory = new ChannelFactory<MobileServiceChannel>();
            factory = CustomUriClientProxy<MobileServiceChannel>.CreateChannelFactory(uri);
            factory.Open();

            var channel = factory.CreateChannel();
            var articles = channel.SearchArticles(medicineName, Language.French);
            decimal price = 0;

            List<Product> products = new List<Product>();

            foreach (var article in articles)
            {
                // Create and add Product objects to the list
                if(article.Prices.Length > 0)
                    price = article.Prices[0].Value;
                else price = 0;
                products.Add(new Product(article.EanCode, article.Description, article.PharmaCode, article.QuantityOnStock, article.InStock, price));
            }

            factory.Close();

            return products;
        }

        // Static method to display products
        public static void DisplayProducts(List<Product> products)
        {
            int counter = 1;
            foreach (var product in products)
            {
                Console.WriteLine($"{counter}. {product}");
                counter++;
            }
        }
    }
}
