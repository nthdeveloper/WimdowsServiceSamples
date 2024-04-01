using System;
using System.Threading;
using log4net;
using WindowsServiceWithWebApi.Api;

namespace WindowsServiceWithWebApi
{
    internal class SampleService
    {
        ILog _logger = LogManager.GetLogger(typeof(SampleService));

        ServiceState _currentState;
        WebApiService _apiService;
        private readonly ServiceSettings _settings;

        public SampleService(ServiceSettings settings)
        {
            _settings = settings;

            _currentState = ServiceState.Stopped;

            _apiService = new WebApiService(_settings.ApiAddress, _settings.ApiPort);            

            Thread thread = new Thread(serviceThread);
            thread.Start();
            
        }

        public void Start()
        {
            _logger.Info("Service started");

            _currentState = ServiceState.Running;

            _apiService.Start();
        }

        public void Pause()
        {
            _logger.Info("Service paused");

            _currentState = ServiceState.Paused;

            _apiService.Stop();
        }

        public void Resume()
        {
            _logger.Info("Service resumed");

            _currentState = ServiceState.Running;

            _apiService.Start();
        }

        public void Stop()
        {
            _logger.Info("Service stopped");

            _currentState = ServiceState.Stopped;

            _apiService.Stop();
        }

        private void serviceThread()
        {
            while (true)
            {
                _logger.Info("Service state: "+_currentState);
                Thread.Sleep(1000);

                if (_currentState == ServiceState.Stopped)
                    return;
            }
        }

        enum ServiceState
        {
            Stopped,
            Running,
            Paused
        }
    }
}
