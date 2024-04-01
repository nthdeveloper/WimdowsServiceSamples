using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using log4net;

namespace WindowsServiceWithWebApi.Api
{
    internal class WebApiService
    {
        ILog _logger = LogManager.GetLogger(typeof(WebApiService));

        private readonly string _address;
        private readonly int _port;

        IDisposable _owinServer;

        public WebApiService(string address, int port)
        {
            _address = address;
            _port = port;
        }

        public void Start()
        {
            string baseUrl = $"http://{_address}:{_port}/";

            // Start OWIN host
            _owinServer = WebApp.Start(baseUrl, (appBuilder) =>
            {
                //Allow all origins
                appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

                // Configure Web API for Self-Host
                HttpConfiguration config = new HttpConfiguration();
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                appBuilder.UseWebApi(config);
            });

            _logger.Info("Web api is running at "+ baseUrl);
        }

        public void Stop()
        {
            if (_owinServer != null)
                _owinServer.Dispose();

            _owinServer = null;
        }
    }
}
