using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Deployment.Helpers;

namespace Deployment
{
    internal class Program
    {
        private static Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// Starts the timer.
        /// </summary>
        private static void StartTimer()
        {
            _stopwatch.Restart(); // Restart ensures the stopwatch starts fresh
            Console.WriteLine("Timer started.");
        }

        /// <summary>
        /// Stops the timer and displays the time taken.
        /// </summary>
        private static void StopAndDisplayTime()
        {
            _stopwatch.Stop(); // Stop the timer
            Console.WriteLine($"Time taken: {_stopwatch.Elapsed}");
        }

        static async Task Main(string[] args)
        {
            // Define the list of server paths
            List<string> serversAll = EnvItem.GetServerList();
            List<string> serversPilot = EnvItem.GetServerListPilot();
            List<string> serversNplus2 = EnvItem.GetServerListNPlus2();
            List<string> serversNplus1Business = EnvItem.GetServerListNPlus1Business();
            List<string> serversNplus1Automation = EnvItem.GetServerListNPlus1Automation();

            bool exit = false;

            while (!exit)
            {
                // Main Menu
                Console.Clear();
                Console.WriteLine("0. Exit");
                Console.WriteLine("******* Main Menu *******");
                Console.WriteLine("1. WebApiClient - IIS");
                Console.WriteLine("2. WebApiClientService");
                Console.WriteLine("3. Integration Test");
                Console.Write("Select an option: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {

                    case "1":
                        await ShowWebApiClientMenu(serversAll, serversPilot, serversNplus2, serversNplus1Business, serversNplus1Automation);
                        break;

                    case "2":
                        await ShowWebApiClientServiceMenu(serversAll, serversPilot, serversNplus2, serversNplus1Business, serversNplus1Automation);
                        break;

                    case "3":
                        await ShowIntegrationTestMenu(serversAll, serversPilot, serversNplus2, serversNplus1Business, serversNplus1Automation);
                        break;

                    case "0":
                        exit = true;
                        Console.WriteLine("Exiting... Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static async Task ShowWebApiClientMenu(
            List<string> serversAll, List<string> serversPilot, List<string> serversNplus2, List<string> serversNplus1Business, List<string> serversNplus1Automation)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("******* WebApiClient - IIS *******");
                Console.WriteLine("1. Pilot: Deploy and Test");
                Console.WriteLine("2. N+2: Deploy and Test");
                Console.WriteLine("3. N+1 Business: Deploy and Test");
                Console.WriteLine("4. N+1 Auto: Deploy and Test");
                Console.WriteLine("5. All servers: Test");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StartTimer();
                        WebApiClient.DeployFolder(serversPilot);
                        await WebApiClient.TestDeploymentAsync(serversPilot);
                        StopAndDisplayTime();
                        break;

                    case "2":
                        StartTimer();
                        WebApiClient.DeployFolder(serversNplus2);
                        await WebApiClient.TestDeploymentAsync(serversNplus2);
                        StopAndDisplayTime();
                        break;

                    case "3":
                        StartTimer();
                        WebApiClient.DeployFolder(serversNplus1Business);
                        await WebApiClient.TestDeploymentAsync(serversNplus1Business);
                        StopAndDisplayTime();
                        break;

                    case "4":
                        StartTimer();
                        WebApiClient.DeployFolder(serversNplus1Automation);
                        await WebApiClient.TestDeploymentAsync(serversNplus1Automation);
                        StopAndDisplayTime();
                        break;

                    case "5":
                        StartTimer();
                        await WebApiClient.TestDeploymentAsync(serversAll);
                        StopAndDisplayTime();
                        break;

                    case "100":
                        StartTimer();
                        WebApiClient.DeployFolder(serversAll);
                        await WebApiClient.TestDeploymentAsync(serversAll);
                        StopAndDisplayTime();
                        break;

                    case "0":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

                if (!back)
                {
                    Console.WriteLine("\nType 'ok' to return to the menu...");

                    string userInput;
                    do
                    {
                        userInput = Console.ReadLine()?.Trim(); // Read user input and trim whitespace
                    } while (!string.Equals(userInput, "ok", StringComparison.OrdinalIgnoreCase));

                    Console.WriteLine("Returning to the menu...");
                }
            }
        }

        static async Task ShowWebApiClientServiceMenu(
            List<string> serversAll, List<string> serversPilot, List<string> serversNplus2, List<string> serversNplus1Business, List<string> serversNplus1Automation)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("******* WebApiClientService *******");
                Console.WriteLine("1. Pilot: Copy files + create updateNow.txt - AMA704");
                Console.WriteLine("2. N+2: Copy files + create updateNow.txt - CVI506, SUN008, SUN007");
                Console.WriteLine("******* WebApiClientServiceUpdater *******");
                Console.WriteLine("50. Pilot: Install service - AMA704");
                Console.WriteLine("51. Pilot: Install service - CVI506, SUN008, SUN007");
                Console.WriteLine("60. Pilot: Restart service - AMA704");
                Console.WriteLine("61. Pilot: Restart service - CVI506, SUN008, SUN007");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StartTimer();
                        WebApiClientService.CopyServiceBinaries(serversPilot);
                        //Create updateNow.txt to trigger update
                        StopAndDisplayTime();
                        break;

                    case "2":
                        StartTimer();
                        WebApiClientService.CopyServiceBinaries(serversNplus2);
                        //Create updateNow.txt to trigger update
                        StopAndDisplayTime();
                        break;

                    case "50":
                        StartTimer();
                        //Uninstall using WebApiClientService
                        await WebApiClientService.SendRemoteCmd(serversPilot, "sc stop WebApiClientServiceUpdater", true);
                        await WebApiClientService.SendRemoteCmd(serversPilot, "sc delete WebApiClientServiceUpdater", true);
                        //Delete and Copy file 
                        WebApiClientService.CopyServiceUpdaterBinaries(serversPilot);
                        //Install
                        await WebApiClientService.SendRemoteCmd(serversPilot,
                            @"sc create WebApiClientServiceUpdater binPath=""C:\QaTools\WebApiClientServiceUpdater\WebApiClientServiceUpdater.exe"" start=auto",
                            true);
                        await WebApiClientService.SendRemoteCmd(serversPilot, "sc start WebApiClientServiceUpdater", true);
                        StopAndDisplayTime();
                        break;

                    case "51":
                        StartTimer();
                        //Uninstall using WebApiClientService
                        await WebApiClientService.SendRemoteCmd(serversNplus2, "sc stop WebApiClientServiceUpdater", true);
                        await WebApiClientService.SendRemoteCmd(serversNplus2, "sc delete WebApiClientServiceUpdater", true);
                        //Delete and Copy file 
                        WebApiClientService.CopyServiceUpdaterBinaries(serversNplus2);
                        //Install
                        await WebApiClientService.SendRemoteCmd(serversNplus2,
                            @"sc create WebApiClientServiceUpdater binPath=""C:\QaTools\WebApiClientServiceUpdater\WebApiClientServiceUpdater.exe"" start=auto",
                            true);
                        await WebApiClientService.SendRemoteCmd(serversNplus2, "sc start WebApiClientServiceUpdater", true);
                        StopAndDisplayTime();
                        break;

                    case "60":
                        StartTimer();
                        //Uninstall using WebApiClientService
                        await WebApiClientService.SendRemoteCmd(serversPilot, "sc stop WebApiClientServiceUpdater", true);
                        await WebApiClientService.SendRemoteCmd(serversPilot, "sc start WebApiClientServiceUpdater", true);
                        await WebApiClientService.SendRemoteCmd(serversPilot, "sc query WebApiClientServiceUpdater", true);
                        StopAndDisplayTime();
                        break;

                    case "61":
                        StartTimer();
                        //Uninstall using WebApiClientService
                        await WebApiClientService.SendRemoteCmd(serversNplus2, "sc stop WebApiClientServiceUpdater", true);
                        await WebApiClientService.SendRemoteCmd(serversNplus2, "sc start WebApiClientServiceUpdater", true);
                        await WebApiClientService.SendRemoteCmd(serversNplus2, "sc query WebApiClientServiceUpdater", true);
                        StopAndDisplayTime();
                        break;

                    case "0":
                        back = true;
                        break;

                    /*case "1000":
                        StartTimer();
                        WebApiClientService.SendRemoteCmd(serversPilot, "dir", true);
                        WebApiClientService.SendRemoteCmd(serversPilot, "dir", false);
                        StopAndDisplayTime();
                        break;*/

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

                if (!back)
                {
                    Console.WriteLine("\nType 'ok' to return to the menu...");

                    string userInput;
                    do
                    {
                        userInput = Console.ReadLine()?.Trim(); // Read user input and trim whitespace
                    } while (!string.Equals(userInput, "ok", StringComparison.OrdinalIgnoreCase));

                    Console.WriteLine("Returning to the menu...");
                }
            }
        }

        static async Task ShowIntegrationTestMenu(
            List<string> serversAll, List<string> serversPilot, List<string> serversNplus2, List<string> serversNplus1Business, List<string> serversNplus1Automation)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("******* Integration Test *******");
                Console.WriteLine("1. All servers: Deploy Resources");
                Console.WriteLine("2. All servers: Deploy NUnit Console");
                Console.WriteLine("**************");
                Console.WriteLine("10. Pilot: Deploy IntegrationTest");
                Console.WriteLine("11. N+2: Deploy IntegrationTest");
                Console.WriteLine("12. N+1 Business: Deploy IntegrationTest");
                Console.WriteLine("13. N+1 Auto: Deploy IntegrationTest");
                Console.WriteLine("**************");
                Console.WriteLine("20. Pilot: Run test and backup Allure");
                Console.WriteLine("21. N+2: Run test and backup Allure");
                Console.WriteLine("22. N+1 Business: Run test and backup Allure");
                Console.WriteLine("23. N+1 Auto: Run test and backup Allure");
                Console.WriteLine("**************");
                Console.WriteLine("97. All servers: Backup Allure and delete remote file");
                Console.WriteLine("98. All servers: Deploy IntegrationTest");
                Console.WriteLine("99. All servers: Run test and backup Allure");
                Console.WriteLine("100. All servers: Deploy, Run test and backup Allure");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StartTimer();
                        IntegrationTest.DeployFolderRessources(serversAll);
                        StopAndDisplayTime();
                        break;

                    case "2":
                        StartTimer();
                        IntegrationTest.DeployFolderNUnitConsole(serversAll);
                        StopAndDisplayTime();
                        break;

                    case "10":
                        StartTimer();
                        IntegrationTest.DeployFolderIntegrationTest(serversPilot);
                        StopAndDisplayTime();
                        break;

                    case "11":
                        StartTimer(); 
                        IntegrationTest.DeployFolderIntegrationTest(serversNplus2);
                        StopAndDisplayTime();
                        break;

                    case "12":
                        StartTimer();
                        IntegrationTest.DeployFolderIntegrationTest(serversNplus1Business);
                        StopAndDisplayTime();
                        break;

                    case "13":
                        StartTimer();
                        IntegrationTest.DeployFolderIntegrationTest(serversNplus1Automation);
                        StopAndDisplayTime(); 
                        break;

                    case "20":
                        StartTimer();
                        await IntegrationTest.RunTest(serversPilot);
                        IntegrationTest.BackupAllureFolder(serversPilot);
                        StopAndDisplayTime();
                        break;

                    case "21":
                        StartTimer();
                        await IntegrationTest.RunTest(serversNplus2);
                        IntegrationTest.BackupAllureFolder(serversNplus2);
                        StopAndDisplayTime();
                        break;

                    case "22":
                        StartTimer(); 
                        await IntegrationTest.RunTest(serversNplus1Business);
                        IntegrationTest.BackupAllureFolder(serversNplus1Business);
                        StopAndDisplayTime();
                        break;

                    case "23":
                        StartTimer();
                        await IntegrationTest.RunTest(serversNplus1Automation);
                        IntegrationTest.BackupAllureFolder(serversNplus1Automation);
                        StopAndDisplayTime();
                        break;

                    case "97":
                        StartTimer();
                        IntegrationTest.BackupAllureFolder(serversAll);
                        StopAndDisplayTime();
                        break;

                    case "98":
                        StartTimer();
                        IntegrationTest.DeployFolderIntegrationTest(serversAll);
                        StopAndDisplayTime();
                        break;

                    case "99":
                        StartTimer();
                        await IntegrationTest.RunTest(serversAll);
                        IntegrationTest.BackupAllureFolder(serversAll);
                        StopAndDisplayTime();
                        break;

                    case "100":
                        StartTimer();
                        IntegrationTest.DeployFolderIntegrationTest(serversAll);
                        await IntegrationTest.RunTest(serversAll);
                        IntegrationTest.BackupAllureFolder(serversAll);
                        StopAndDisplayTime();
                        break;

                    case "0":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

                if (!back)
                {
                    Console.WriteLine("\nType 'ok' to return to the menu...");

                    string userInput;
                    do
                    {
                        userInput = Console.ReadLine()?.Trim(); // Read user input and trim whitespace
                    } while (!string.Equals(userInput, "ok", StringComparison.OrdinalIgnoreCase));

                    Console.WriteLine("Returning to the menu...");
                }
            }
        }
    }
}
