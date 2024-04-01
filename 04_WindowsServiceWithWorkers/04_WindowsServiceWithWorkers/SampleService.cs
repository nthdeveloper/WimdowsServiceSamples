using System;
using System.Threading;
using log4net;
using WindowsServiceWithWorkers.Data;
using WindowsServiceWithWorkers.Web;

namespace WindowsServiceWithWorkers
{
    internal class SampleService
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(SampleService));
        readonly ServiceSettings _settings;
        ServiceState _currentState;        
        WebService _webService;
        TaskManager _taskManager;
        TaskQueue _taskQueue;        

        public SampleService(ServiceSettings settings)
        {
            _settings = settings;

            _currentState = ServiceState.Stopped;

            _taskQueue = new TaskQueue();
            _taskManager = new TaskManager(_taskQueue);
            _taskManager.MaxConcurrentTasks = _settings.MaxConcurrentTasks;
            _webService = new WebService(_settings.ApiAddress, _settings.ApiPort);

            ServiceLocator.Register<TaskQueue>(_taskQueue);
            ServiceLocator.Register<TaskManager>(_taskManager);

            Thread thread = new Thread(serviceThread);
            thread.Start();
        }

        public void Start()
        {
            _logger.Info("Service started");

            _currentState = ServiceState.Running;

            _taskManager.Start();
            _webService.Start();
        }

        public void Pause()
        {
            _logger.Info("Service paused");

            _currentState = ServiceState.Paused;

            _webService.Stop();
            _taskManager.Stop();
        }

        public void Resume()
        {
            _logger.Info("Service resumed");

            _currentState = ServiceState.Running;
            
            _webService.Start();
            _taskManager.Start();
        }

        public void Stop()
        {
            _logger.Info("Service stopped");

            _currentState = ServiceState.Stopped;

            _webService.Stop();
            _taskManager.Stop();
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
