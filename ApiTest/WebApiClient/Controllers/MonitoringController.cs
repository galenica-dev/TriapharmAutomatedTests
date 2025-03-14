using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApiClient.Models;


namespace WebApiClient.Controllers
{
    public class MonitoringController : Controller
    {
        // GET: Monitoring
        public ActionResult Index()
        {
            return View();
        }

        // Method to calculate the date based on the version
        /*private DateTime CalculateDateFromVersion(string version)
        {
            // Extract the "318" part from the version string
            string dayOfYearString = version.Split('.')[3].Substring(0, 3);
            int dayOfYear = int.Parse(dayOfYearString);

            // Extract the "24" part from the version string as the year
            string yearString = version.Split('.')[2];
            int year = 2000 + int.Parse(yearString); // Assuming the year is in the 2000s

            // Calculate the date by adding dayOfYear to the start of the year
            DateTime startOfYear = new DateTime(year, 1, 1);
            DateTime calculatedDate = startOfYear.AddDays(dayOfYear - 1);

            return calculatedDate;
        }*/

        private DateTime CalculateDateFromVersion(string version)
        {
            // Split the version string
            string[] versionParts = version.Split('.');

            // Extract the last segment (e.g., "701" or "34701")
            string lastSegment = versionParts[3];

            // Remove the last 2 digits and use the remaining digits
            string dayOfYearString = lastSegment.Substring(0, lastSegment.Length - 2);
            int dayOfYear = int.Parse(dayOfYearString);

            // Extract the "24" part from the version string as the year
            string yearString = versionParts[2];
            int year = 2000 + int.Parse(yearString); // Assuming the year is in the 2000s

            // Calculate the date by adding dayOfYear to the start of the year
            DateTime startOfYear = new DateTime(year, 1, 1);
            DateTime calculatedDate = startOfYear.AddDays(dayOfYear - 1);

            return calculatedDate;
        }




        public async Task<ActionResult> PharmacyInfo()
        {
            // N+2
            string apiUrlama704 = "http://SWAMA704VM02.CENTRALINFRA.NET/WebApiClient/LocalService/PharmacyInfoJson";
            string apiUrlsun007 = "http://SWSUN007VM01.centralinfra.net/WebApiClient/LocalService/PharmacyInfoJson";
            string apiUrlsun008 = "http://SWSUN008VM01.CENTRALINFRA.NET/WebApiClient/LocalService/PharmacyInfoJson";
            string apiUrlcvi506 = "http://SWCVI506VM01.CENTRALINFRA.NET/WebApiClient/LocalService/PharmacyInfoJson";

            // N+1 Automation
            string apiUrlama888 = "http://SWAMA888VM01.centralinfra.net/WebApiClient/LocalService/PharmacyInfoJson";
            string apiUrlsun888 = "http://SWSUN888VM01.centralinfra.net/WebApiClient/LocalService/PharmacyInfoJson";
            string apiUrlcvi888 = "http://SWCVI888VM01.centralinfra.net/WebApiClient/LocalService/PharmacyInfoJson";

            // N+1 business
            string apiUrlama705 = "http://SWAMA705VM01.centralinfra.net/WebApiClient/LocalService/PharmacyInfoJson";
            string apiUrlama707 = "http://SWAMA707VM01.centralinfra.net/WebApiClient/LocalService/PharmacyInfoJson";
            string apiUrlcvi503 = "http://SWCVI503VM01.centralinfra.net/WebApiClient/LocalService/PharmacyInfoJson";
            string apiUrlcvi504 = "http://SWCVI504VM01.centralinfra.net/WebApiClient/LocalService/PharmacyInfoJson";
            string apiUrlsun004 = "http://SWSUN004VM01.centralinfra.net/WebApiClient/LocalService/PharmacyInfoJson";
            string apiUrlsun006 = "http://SWSUN006VM01.centralinfra.net/WebApiClient/LocalService/PharmacyInfoJson";

            // Add all URLs to the list
            var urls = new List<string>
            {
                apiUrlama704, apiUrlsun007, apiUrlsun008, apiUrlcvi506, apiUrlama888, apiUrlsun888, apiUrlcvi888,
                apiUrlama705, apiUrlcvi504, apiUrlsun006, apiUrlama707, apiUrlcvi503, apiUrlsun004
            };

            var pharmacyInfoList = new List<PharmacyInfoViewModel>();

            using (HttpClient client = new HttpClient())
            {
                foreach (var url in urls)
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var pharmacyInfo = JsonConvert.DeserializeObject<PharmacyInfoViewModel>(json);
                            if (pharmacyInfo != null)
                            {
                                // Calculate the date based on the version
                                pharmacyInfo.VersionDate = CalculateDateFromVersion(pharmacyInfo.PharmacyVersion).ToString("yyyy-MM-dd");
                                pharmacyInfoList.Add(pharmacyInfo);
                            }
                        }
                    }
                }
            }

            return View(pharmacyInfoList);
        }

    }
}