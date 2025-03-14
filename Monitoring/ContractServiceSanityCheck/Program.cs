using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ContractServiceSanityCheck
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionStringActivePosRead = "Server=localhost;Database=ActivePos_read;Integrated Security=true;";
            var repository = new WebServiceRepository(connectionStringActivePosRead);
            var serviceChecker = new ServiceChecker();
            string filePathFullLog = @"C:\Temp\ContractServiceSanityCheck.txt";
            string filePathErrorLog = @"C:\Temp\ContractServiceSanityCheck_Errors.txt";
            var fullLog = new FileWriter(filePathFullLog);
            var errorLog = new FileWriter(filePathErrorLog);
            string pharmacyVersion = await PharmacyInfo.GetPharmacyVersion();
            // Get the current assembly and Get the version information
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            List<WebService> webServices = repository.GetWebServices(pharmacyVersion, version.ToString());
            List<string> codeForErrorReport = new List<string>();
            int nbOfError = 0;
            
            

            foreach (var service in webServices)
            {
                string httpResponse;
                //bool isAvailable;

                // Assuming URI format to determine if it's HTTP or TCP
                if (service.Uri.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    httpResponse = await serviceChecker.IsHttpServiceAvailable(service.Uri, service.Login, service.Password);
                    Console.Write($"URI: {service.Uri}, Status: ");
                    if (httpResponse!= "N/A")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{httpResponse}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{httpResponse}");
                        errorLog.WriteLine($"{service.Uri};{service.Code};{httpResponse};{service.TriaPharmVersion};{service.ContractServiceSanityCheckVersion};");
                        codeForErrorReport.Add( service.Code);
                        nbOfError += 1;
                    }
                    fullLog.WriteLine($"{service.Uri};{service.Code};{httpResponse};{service.TriaPharmVersion};{service.ContractServiceSanityCheckVersion}");

                    // Reset color to default
                    Console.ResetColor();
                }
                /*else
                {
                    var uri = new Uri(service.Uri);
                    isAvailable = serviceChecker.IsTcpServiceAvailable(uri.Host, uri.Port);
                    Console.WriteLine($"{service.Uri};{isAvailable}");
                }*/


            }

            // Error counts
            Console.Write($"\nNumber of services to monitor: {webServices.Count} with ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{nbOfError}");
            Console.ResetColor();
            Console.WriteLine($" not reachable service(s)");

            // Display Code error report
            Console.WriteLine("\nReport Analysis:");
            CodeReport report = new CodeReport(codeForErrorReport);
            var results = report.GenReport();
            foreach (var result in results) 
            {
                Console.WriteLine($"-> {result}");
            }

            //Display version and wait for closing window
            Console.WriteLine($"\nVersion {version}");
            //Console.WriteLine("\nPress any key to return to the menu...");
            //Console.ReadKey();

            serviceChecker.Dispose();
        }
    }

}
