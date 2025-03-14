using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Owin;

namespace PersonApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Configure JSON serialization to use kebab-case
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new KebabCaseNamingStrategy()
            };
            jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}
