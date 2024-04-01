using System;
using System.Threading;
using log4net;

namespace SimpleWimdowsService
{
    internal class SampleService
    {
        ILog _logger = LogManager.GetLogger(typeof(SampleService));

        ServiceState _currentState;

        public SampleService()
        {
            _currentState = ServiceState.Stopped;

            Thread thread = new Thread(serviceThread);
            thread.Start();
        }

        public void Start()
        {
            _logger.Info("Service started");

            _currentState = ServiceState.Running;
        }

        public void Pause()
        {
            _logger.Info("Service paused");

            _currentState = ServiceState.Paused;
        }

        public void Resume()
        {
            _logger.Info("Service resumed");

            _currentState = ServiceState.Running;
        }

        public void Stop()
        {
            _logger.Info("Service stopped");

            _currentState = ServiceState.Stopped;
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
