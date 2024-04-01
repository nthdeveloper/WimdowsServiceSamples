using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using log4net;
using System.Threading;
using WindowsServiceWithWorkers.Data;
using System.Net.Http.Headers;
using System.Runtime;

namespace WindowsServiceWithWorkers
{
    internal class TaskManager
    {
        readonly ILog _log = LogManager.GetLogger(typeof(TaskManager));
        readonly List<TaskProcessor> _taskProcessors = new List<TaskProcessor>(8);
        readonly TaskQueue _taskQueue;
        readonly object _syncObj = new object();
        Thread _managerThread;
        bool _isRunning;

        public int MaxConcurrentTasks { get; set; } = 2;

        public TaskManager(TaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }

        public void Start()
        {
            _log.Info("Starting");

            lock (_syncObj)
            {
                _isRunning = true;
                _managerThread = new Thread(managerThread);
                _managerThread.Name = "ManagerThread";
                _managerThread.Start();
            }
        }
        public void Stop()
        {
            _log.Info("Stopping");

            lock (_syncObj)
            {
                _isRunning = false;

                while (_taskProcessors.Count > 0)
                {
                    var processor = _taskProcessors[0];
                    processor.CancelTask();
                    _taskProcessors.Remove(processor);
                }
            }
        }

        private void managerThread()
        {
            while (_isRunning)
            {
                Thread.Sleep(1000);
                if (!_isRunning) break;

                lock (_syncObj)
                {
                    if (_taskProcessors.Count >= this.MaxConcurrentTasks) continue;
                }

                try
                {
                    var taskItem = _taskQueue.GetNextTask();
                    if (taskItem != null)
                    {
                        var taskProcessor = new TaskProcessor(taskItem);
                        taskProcessor.TaskCompleted += TaskProcessor_TaskCompleted;

                        lock (_syncObj)
                        {
                            _taskProcessors.Add(taskProcessor);
                        }

                        taskProcessor.ProcessTask();
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            }
        }

        private void TaskProcessor_TaskCompleted(object sender, TaskProcessor processor)
        {
            lock (_syncObj)
            {
                if (_taskProcessors.Contains(processor))
                    _taskProcessors.Remove(processor);
            }
        }
    }
}
