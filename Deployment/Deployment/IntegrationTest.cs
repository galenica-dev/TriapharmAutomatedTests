using Deployment.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment
{
    public static class IntegrationTest
    {
        public static void DeployFolderRessources(List<string> servers)
        {
            string sourceFolder = @"C:\QaTools\Ressources";

            foreach (string server in servers)
            {
                string destinationFolder = $@"\\{server}\c$\QaTools\Ressources";

                // Delete destination folder content
                FileSys.DeleteFolderContent(destinationFolder);

                // Copy content from source to destination
                FileSys.CopyFolderContent(sourceFolder, destinationFolder);

                LogConsole.Log($"Deployment to server {server} completed.");
            }
        }


        public static void DeployFolderNUnitConsole(List<string> servers)
        {
            string sourceFolder = @"C:\QaTools\nunit";

            foreach (string server in servers)
            {
                string destinationFolder = $@"\\{server}\c$\QaTools\nunit";

                // Delete destination folder content
                FileSys.DeleteFolderContent(destinationFolder);

                // Copy content from source to destination
                FileSys.CopyFolderContent(sourceFolder, destinationFolder);

                LogConsole.Log($"Deployment to server {server} completed.");
            }
        }

        public static void DeployFolderIntegrationTest(List<string> servers)
        {
            string sourceFolder = @"C:\repo\TriapharmAutomatedTests\IntegrationTest\IntegrationTest\bin\Debug\net8.0";

            foreach (string server in servers)
            {
                string destinationFolder = $@"\\{server}\c$\QaTools\IntegrationTest";

                // Delete destination folder content
                FileSys.DeleteFolderContent(destinationFolder);

                // Copy content from source to destination
                FileSys.CopyFolderContent(sourceFolder, destinationFolder);

                Console.ForegroundColor = ConsoleColor.Green;
                LogConsole.Log($"Deployment to server {server} completed.");
                Console.ResetColor();
            }
        }

        public static async Task RunTest(List<string> servers, string testProject = "IntegrationTest")
        {
            foreach (string server in servers)
            {
                bool success = await RunCmd.Execute(server, $@"cd {testProject} && C:\QaTools\nunit\nunit3-console.exe IntegrationTest.dll");

                if (success)
                {
                    LogConsole.Log($"The process on {server} was terminated successfully.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    LogConsole.Log($"Failed to terminate the process on {server}");
                    Console.ResetColor();
                }

            }
        }

        public static void BackupAllureFolder(List<string> serversMonitoring, string testProject = "IntegrationTest")
        {
            foreach (var server in serversMonitoring)
            {
                try
                {
                    // Define source paths on the server
                    string sourceBasePath = $@"\\{server}\c$\QaTools\{testProject}\allure-results";

                    // Define destination base path
                    string destinationBasePath = $@"C:\QaTools\allure-reports\IntegrationTest\{testProject}\{server}";

                    // Ensure the destination directory exists
                    Directory.CreateDirectory(destinationBasePath);

                    // Copy folder content
                    FileSys.CopyFolderContent(sourceBasePath, destinationBasePath);

                    // Delete content of the source folder while keeping it empty
                    DeleteFolderContent(sourceBasePath);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    LogConsole.Log($"Failed to back up files from {server}: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// Deletes all content of a folder but keeps the folder itself.
        /// </summary>
        /// <param name="folderPath">The path to the folder to be cleared.</param>
        private static void DeleteFolderContent(string folderPath)
        {
            // Delete all files
            foreach (var file in Directory.EnumerateFiles(folderPath))
            {
                File.Delete(file);
            }

            // Delete all subdirectories
            foreach (var directory in Directory.EnumerateDirectories(folderPath))
            {
                Directory.Delete(directory, true); // Recursive deletion
            }
            LogConsole.Log($"Delete content in {folderPath}");
        }
    }
}
