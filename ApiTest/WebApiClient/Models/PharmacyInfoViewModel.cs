using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiClient.Models
{
    public class PharmacyInfoViewModel
    {
        public string OuCode { get; set; }
        public int OuId { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyVersion { get; set; }
        public string Language { get; set; }
        public string PharmacyGln { get; set; }
        public string VersionDate { get; set; }
    }
}