using Deployment.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deployment
{
    public static class ContractServiceSanityCheck
    {
        public static string GetVersion()
        {
            return ExeVersion.GetVersion(@"C:\repo\TriapharmAutomatedTests\Monitoring\ContractServiceSanityCheck\bin\Release\ContractServiceSanityCheck.exe");
        }

        public static void BackupFilesMonitoring(List<string> serversMonitoring)
        {
            // List to store contents from each server's ContractServiceSanityCheck.txt file, starting with the header
            List<string> allContents = new List<string>
            {
                "Date;Host;url;code;status;TriaPharmVersion;ContractServiceSanityCheckVersion;" // Header line
            };

            foreach (var server in serversMonitoring)
            {
                try
                {
                    // Define source paths on the server
                    string sourceBasePath = $@"\\{server}\c$\Temp";
                    string[] sourceFiles =
                    {
                        "ContractServiceSanityCheck.txt",
                        "ContractServiceSanityCheck_Errors.txt",
                        "ContractServiceSanityCheck_ExcludedUris.txt"
                    };

                    // Define destination base path
                    string destinationBasePath = $@"C:\repo\logs\Monitoring\{server}";

                    // Ensure the destination directory exists
                    Directory.CreateDirectory(destinationBasePath);

                    foreach (var fileName in sourceFiles)
                    {
                        string sourceFilePath = Path.Combine(sourceBasePath, fileName);
                        string destinationFilePath = Path.Combine(destinationBasePath, fileName);

                        if (File.Exists(sourceFilePath))
                        {
                            File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
                            LogConsole.Log($"Backed up {fileName} from {server} to {destinationFilePath}");

                            // If the file is ContractServiceSanityCheck.txt, add its contents to the list
                            if (fileName == "ContractServiceSanityCheck.txt")
                            {
                                allContents.AddRange(File.ReadAllLines(sourceFilePath));
                            }
                        }
                        else
                        {
                            LogConsole.Log($"Source file does not exist: {sourceFilePath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogConsole.Log($"Failed to back up files from {server}: {ex.Message}");
                }
            }

            // Write all contents to a single file
            string allContentsFilePath = $@"C:\repo\logs\Monitoring\ContractServiceSanityCheck_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.csv";
            File.WriteAllLines(allContentsFilePath, allContents);
        }

        public static void PrepareDeploymentMonitoring()
        {
            string sourceFilePath = @"C:\repo\TriapharmAutomatedTests\Monitoring\ContractServiceSanityCheck\ExcludedUris.txt";
            string destinationFolderPath = @"C:\repo\deploy\Monitoring";
            string destinationFileName = "ContractServiceSanityCheck_ExcludedUris.txt";
            string destinationFilePath = Path.Combine(destinationFolderPath, destinationFileName);

            File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
        }

        public static void ResetLog(List<string> servers)
        {
            foreach (string server in servers)
            {
                FileSys.DeleteFile($@"\\{server}\c$\Temp\ContractServiceSanityCheck.txt");
                FileSys.DeleteFile($@"\\{server}\c$\Temp\ContractServiceSanityCheck_Errors.txt");
            }
        }

        public static async Task TaskKillAsync(List<string> servers)
        {
            foreach (string server in servers)
            {
                bool success = await RunCmd.KillTaskAsync(server, "ContractServiceSanityCheck.exe");

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

        public static async Task RunMonitor(List<string> servers) 
        {
            foreach (string server in servers)
            {
                bool success = await RunCmd.Execute(server, @"cd ContractServiceSanityCheck && ContractServiceSanityCheck.exe");

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

        public static void DeployFilesMonitoring(List<string> servers)
        {
            string sourceFolder = @"C:\repo\TriapharmAutomatedTests\Monitoring\ContractServiceSanityCheck\bin\Release";
            string sourceExcludedUrisFile = @"C:\repo\deploy\Monitoring\ContractServiceSanityCheck_ExcludedUris.txt";


            foreach (string server in servers)
            {
                string destinationFolder = $@"\\{server}\c$\QaTools\ContractServiceSanityCheck";
                string destinationTempFolderPath = $@"\\{server}\c$\Temp";
                string destinationExcludedUrisFile = Path.Combine(destinationTempFolderPath, "ContractServiceSanityCheck_ExcludedUris.txt");

                // Delete destination folder content
                FileSys.DeleteFolderContent(destinationFolder);

                // Copy content from source to destination
                FileSys.CopyFolderContent(sourceFolder, destinationFolder);

                //ExcludedUris part
                if (File.Exists(destinationExcludedUrisFile))
                {
                    File.Delete(destinationExcludedUrisFile);
                }
                File.Copy(sourceExcludedUrisFile, destinationExcludedUrisFile);
                LogConsole.Log($"Text file copied to: {destinationExcludedUrisFile}");

                LogConsole.Log($"Deployment to server {server} completed.");
            }
        }
    }
}
