using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Deployment.Helpers
{
    public static class FileSys
    {
        public static void DeleteFolderContent(string folderPath)
        {
            try
            {
                LogConsole.Log($"Start to delete content of folder: {folderPath}");
                if (Directory.Exists(folderPath))
                {
                    DirectoryInfo directory = new DirectoryInfo(folderPath);

                    // Delete all files
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        file.IsReadOnly = false; // Remove read-only attribute if necessary
                        file.Delete();
                    }

                    // Delete all directories
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                    {
                        dir.Delete(true); // True to recursively delete all children
                    }

                    LogConsole.Log($"Deleted content of folder: {folderPath}");
                }
                else
                {
                    Directory.CreateDirectory(folderPath);
                    LogConsole.Log($"Created destination folder: {folderPath}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                LogConsole.Log($"Failed to delete content of folder: {folderPath}. Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void CopyFolderContent(string sourceFolder, string destinationFolder)
        {
            try
            {
                LogConsole.Log($"Start to copy content from {sourceFolder} to {destinationFolder}");
                foreach (string dirPath in Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(sourceFolder, destinationFolder));
                }

                foreach (string filePath in Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(filePath, filePath.Replace(sourceFolder, destinationFolder), true);
                }
                LogConsole.Log($"Copied content from {sourceFolder} to {destinationFolder}");   
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                LogConsole.Log($"Failed to copy content to {destinationFolder}. Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        public static void DeleteFile(string filePath)
        {
            try
            {
                LogConsole.Log($"Attempting to delete file: {filePath}");

                if (File.Exists(filePath))
                {
                    FileInfo file = new FileInfo(filePath);

                    if (file.IsReadOnly)
                    {
                        file.IsReadOnly = false; // Remove read-only attribute if necessary
                    }

                    file.Delete();
                    LogConsole.Log($"File deleted successfully: {filePath}");
                }
                else
                {
                    LogConsole.Log($"File does not exist: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                LogConsole.Log($"Failed to delete file: {filePath}. Error: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
