using System.IO;
using System.IO.Compression;

namespace Deployment.Helpers
{
    public static class FolderCompressor
    {
        /// <summary>
        /// Compresses a folder into a zip file.
        /// </summary>
        /// <param name="sourceFolderPath">The path to the folder to compress.</param>
        /// <param name="destinationZipFilePath">The path where the zip file should be created.</param>
        public static void CompressFolder(string sourceFolderPath, string destinationZipFilePath)
        {
            if (!Directory.Exists(sourceFolderPath))
            {
                throw new DirectoryNotFoundException($"Source folder does not exist: {sourceFolderPath}");
            }

            // Check if the destination zip file already exists and delete it if it does
            if (File.Exists(destinationZipFilePath))
            {
                File.Delete(destinationZipFilePath);
            }

            // Compress the folder into the zip file
            ZipFile.CreateFromDirectory(sourceFolderPath, destinationZipFilePath, CompressionLevel.Optimal, false);
        }
    }
}
