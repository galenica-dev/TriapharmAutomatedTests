using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFromDb
{
    public class ProductLinks
    {
        public string OriginalItemId { get; set; }
        public string OriginalGtinEanCode { get; set; }
        public string OriginalPharmacode { get; set; }
        public string OriginalDescription { get; set; }
        public string LinkedItemId { get; set; }
        public string LinkedGtinEanCode { get; set; }
        public string LinkedPharmacode { get; set; }
        public string LinkedDescription { get; set; }
        public int? LinkType { get; set; }
        public DateTime? LinkStartDate { get; set; }
        public DateTime? LinkEndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string Error { get; set; }

        // Override ToString to provide a meaningful representation of the object
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"OriginalItemId: {OriginalItemId}");
            sb.AppendLine($"OriginalGtinEanCode: {OriginalGtinEanCode}");
            sb.AppendLine($"OriginalPharmacode: {OriginalPharmacode}");
            sb.AppendLine($"OriginalDescription: {OriginalDescription}");
            sb.AppendLine($"LinkedItemId: {LinkedItemId}");
            sb.AppendLine($"LinkedGtinEanCode: {LinkedGtinEanCode}");
            sb.AppendLine($"LinkedPharmacode: {LinkedPharmacode}");
            sb.AppendLine($"LinkedDescription: {LinkedDescription}");
            sb.AppendLine($"LinkType: {LinkType}");
            sb.AppendLine($"LinkStartDate: {LinkStartDate}");
            sb.AppendLine($"LinkEndDate: {LinkEndDate}");
            sb.AppendLine($"IsActive: {IsActive}");
            sb.AppendLine($"Error: {Error}");
            return sb.ToString();
        }

    }


}
