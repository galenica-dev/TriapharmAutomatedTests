using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFromDb;

namespace eRxCHMED16Service
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the ApiClientConvertToChmed16A class
            var apiClient = new ApiClientConvertToChmed16A();
            var customerService = new CustomerService();
            string connectionStringActivePosRead = "Server=.;Database=ActivePos_read;Integrated Security=true;";
            var customers = customerService.GetCustomerWithValidInsurance(connectionStringActivePosRead, 1);

            // Call the method to send the POST request and get the response
            var response = apiClient.SendPostRequest(customers[0], ApiClientConvertToChmed16A.Scenario.HappyPath).GetAwaiter().GetResult();

            // Access the status code
            Console.WriteLine($"Response Code: {response.StatusCode}");

            // Optionally, print the response body as well
            var responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Console.WriteLine($"Response Body: {responseBody}");

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();  // Waits for the user to press a key before clearing the screen
        }
    }
}
