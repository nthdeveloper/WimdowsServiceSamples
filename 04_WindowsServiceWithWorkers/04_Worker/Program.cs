using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using log4net;
using System.Diagnostics;
using System.Threading;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace WindowsServiceWithWorkers
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            //Uncomment to be able to debug worker process from the beginning
            /* 
#if DEBUG
            while (!Debugger.IsAttached)
            {
                Thread.Sleep(1000);
            }
#endif
            */

            Guid _taskId = Guid.Parse(args[0]);
            string _taskName = args[1];

            ILog _log = setupLogger(args[0]);

            var _worker = new Worker();

            try
            {
                _worker.ProcessTask(_taskId, _taskName);
            }
            catch(Exception ex)
            {
                _log.Error("Worker failed.", ex);
                return -1;
            }

            _log.Info("Worker ended.");

            Thread.Sleep(2000);

            return 0;
        }

        private static ILog setupLogger(string name)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();

            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = false;
            roller.File = $"WorkerLogs\\{DateTime.Today.ToString("yyyyMMdd")}\\{name}_{DateTime.Now.ToString("HH_mm_ss")}.txt";
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 30;
            roller.MaximumFileSize = "10MB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Date;
            roller.StaticLogFileName = true;
            roller.AppendToFile = true;

            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;

            return LogManager.GetLogger(typeof(Program));
        }
    }
}
