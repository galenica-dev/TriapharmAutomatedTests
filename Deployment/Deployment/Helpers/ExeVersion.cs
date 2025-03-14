using System;
using System.Diagnostics;
using System.IO;

namespace Deployment.Helpers
{
    public static class ExeVersion
    {
        /// <summary>
        /// Retrieves the version number from an executable file.
        /// </summary>
        /// <param name="exeFilePath">The path to the executable file.</param>
        /// <returns>The version number as a string, or null if it could not be retrieved.</returns>
        public static string GetVersion(string exeFilePath)
        {
            if (!File.Exists(exeFilePath))
            {
                Console.WriteLine($"Executable file not found: {exeFilePath}");
                return null;
            }

            var versionInfo = FileVersionInfo.GetVersionInfo(exeFilePath);
            return versionInfo.FileVersion;
        }
    }
}
