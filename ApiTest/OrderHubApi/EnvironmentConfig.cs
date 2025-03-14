using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHubApi
{
    public class EnvironmentConfig
    {
        public string ClientId { get; set; }
        public string ClientIdRx { get; set; }
        public string ClientSecret { get; set; }
        public string ClientSecretRx { get; set; }
        public string Scope { get; set; }
        public string UrlToGetToken { get; set; }
        public string SubKey { get; set; }
        public string SubKeyRx { get; set; }
        public string UrlPostRequest { get; set; }
    }
}
