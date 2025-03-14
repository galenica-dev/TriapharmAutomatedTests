using System;
using System.ServiceProcess;
using Microsoft.Owin.Hosting;

namespace PersonApi
{
    public partial class PersonApiService: ServiceBase
    {

        public PersonApiService()
        {
            //InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _webApp = WebApp.Start<Startup>(BaseAddress);
            Console.WriteLine($"PersonApi service running at {BaseAddress}");
        }

        protected override void OnStop()
        {
            _webApp?.Dispose();
            Console.WriteLine("PersonApi service stopped.");
        }
    }
}
