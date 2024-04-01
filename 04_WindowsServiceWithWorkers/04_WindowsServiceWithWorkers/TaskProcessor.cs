using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsServiceWithWorkers.Data;

namespace WindowsServiceWithWorkers
{
    internal class TaskProcessor
    {
        ILog _log = LogManager.GetLogger(typeof(TaskProcessor));

        Process _workerProcess;
        private readonly TaskItem _taskItem;

        public TaskItem TaskItem => _taskItem;

        public int ExitCode 
        {
            get
            {
                return _workerProcess != null ? _workerProcess.ExitCode : 0;
            }        
        }

        public event EventHandler<TaskProcessor> TaskCompleted;

        public TaskProcessor(TaskItem taskItem)
        {
            _taskItem = taskItem;
        }

        public void ProcessTask()
        {
            _log.Info("Processing task. Task: " + _taskItem.Name);

            runWorkerProcess();

            _log.Info("Finished task. Task: " + _taskItem.Name);
        }

        public void CancelTask()
        {
            if (_workerProcess != null)
                _workerProcess.Kill();
        }                       

        private bool runWorkerProcess()
        {
            ProcessStartInfo _startInfo = new ProcessStartInfo();
            _startInfo.Arguments = $"{_taskItem.Id} \"{_taskItem.Name}\"" ;
            _startInfo.CreateNoWindow = true;
            _startInfo.ErrorDialog = false;
            _startInfo.FileName = "Worker.exe";
            _startInfo.UseShellExecute = false;

            _workerProcess = new Process();
            _workerProcess.StartInfo = _startInfo;
            _workerProcess.EnableRaisingEvents = true;
            _workerProcess.Exited += new EventHandler(WorkerProcess_Exited);

            bool result = _workerProcess.Start();

            return result;
        }

        private void WorkerProcess_Exited(object sender, EventArgs e)
        {
            this.TaskCompleted?.Invoke(this, this);            
        }
    }
}
