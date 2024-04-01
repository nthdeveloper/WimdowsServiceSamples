using Owin;
using System;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using log4net;

namespace WindowsServiceWithWorkers.Web
{
    internal class WebService
    {
        ILog _logger = LogManager.GetLogger(typeof(WebService));

        private readonly string _address;
        private readonly int _port;

        IDisposable _owinServer;

        public WebService(string address, int port)
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
                    routeTemplate: "api/{controller}/{action}",
                    defaults: new { id = RouteParameter.Optional }
                );

                //Add Web API middleware
                appBuilder.UseWebApi(config);

                //Add web UI middleware
                var physicalFileSystem = new PhysicalFileSystem(@"./Web");

                var options = new FileServerOptions
                {
                    EnableDefaultFiles = true,
                    FileSystem = physicalFileSystem
                };

                options.StaticFileOptions.FileSystem = physicalFileSystem;
                options.StaticFileOptions.ServeUnknownFileTypes = true;
                options.DefaultFilesOptions.DefaultFileNames = new[] { "index.html" };

                appBuilder.UseFileServer(options);
            });

            _logger.Info("Web service is running at "+ baseUrl);
        }

        public void Stop()
        {
            if (_owinServer != null)
                _owinServer.Dispose();

            _owinServer = null;
        }
    }
}
