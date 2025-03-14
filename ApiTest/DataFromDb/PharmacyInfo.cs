using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace DataFromDb
{
    public static class PharmacyInfo
    {
        // Static list of rows with ouCode and glnCode
        private static readonly List<(string ouCode, string glnCode)> CodeMapping = new List<(string ouCode, string glnCode)>
        {
            ("007", "7601001370944"),
            ("008", "7601001398481"),
            ("506", "7601001002135"),
            ("704", "7601001362383"),
            ("705", "7601001362383"),
            ("504", "7601001369566"),
            ("006", "7601001398481"),
            ("707", "7601001368781"),
            ("503", "7601001002135"),
            ("004", "7601001370944"),
            // Add more mappings as needed
            // 888 WILL BE NULL because they could not have GLN
        };

        /// <summary>
        /// Retrieves the GLN code for the specified OU code.
        /// </summary>
        /// <param name="ouCode">The OU code to look up.</param>
        /// <returns>The corresponding GLN code, or null if not found.</returns>
        public static string GetPharmacyGLN(string ouCode)
        {
            // Find the matching row based on ouCode
            var match = CodeMapping.FirstOrDefault(row => row.ouCode == ouCode);
            return match.glnCode; // Return the glnCode, or null if not found
        }

    }
}
