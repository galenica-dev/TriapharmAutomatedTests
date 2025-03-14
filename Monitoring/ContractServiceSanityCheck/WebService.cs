using System;

namespace ContractServiceSanityCheck
{
    public class WebService
    {
        public Guid WebserviceId { get; set; }
        public string Uri { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string CompanyCode { get; set; } 
        public string Code { get; set; }
        public string TriaPharmVersion { get; set; }
        public string ContractServiceSanityCheckVersion { get; set; }
    }
}
