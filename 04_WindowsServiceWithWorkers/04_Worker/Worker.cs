using log4net;
using System;
using System.Threading;

namespace WindowsServiceWithWorkers
{
    internal class Worker
    {
        ILog _log = LogManager.GetLogger(typeof(Worker));

        public void ProcessTask(Guid taskId, string taskName)
        {
            _log.Info($"Processing task. Id:{taskId}, Name:{taskName}");

            Thread.Sleep(3000);

            _log.Info($"Task completed. Id:{taskId}, Name:{taskName}");
        }
    }
}
