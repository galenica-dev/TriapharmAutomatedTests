using System;
using System.ServiceProcess;
using Microsoft.Owin.Hosting;

namespace PersonApi
{
    public partial class PersonApiService : ServiceBase
    {
        private IDisposable _webApp;
        private const string BaseAddress = "http://localhost:5001/";
    }
}