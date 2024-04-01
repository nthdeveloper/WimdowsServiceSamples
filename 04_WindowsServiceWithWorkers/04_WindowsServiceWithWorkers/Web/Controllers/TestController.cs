using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using log4net;
using WindowsServiceWithWorkers.Web.Models;
using WindowsServiceWithWorkers.Data;

namespace WindowsServiceWithWorkers.Web.Controllers
{
    public class TestController : ApiController
    {
        ILog _log = LogManager.GetLogger(typeof(TestController));

        [HttpGet]
        public int Sum(int num1, int num2)
        {
            return num1 + num2;
        }

        [HttpGet]
        public SystemInfo SystemInfo()
        {
            var sysInfo = new SystemInfo();
            sysInfo.MachineName = Environment.MachineName;
            sysInfo.UserName = Environment.UserName;
            sysInfo.OSVersion = Environment.OSVersion.ToString();
            sysInfo.WorkingSet = Environment.WorkingSet;

            return sysInfo;
        }

        [HttpPost]
        public TaskItem AddTask(AddTaskModel task)
        {
            var taskQueue= ServiceLocator.Get<TaskQueue>();
            var taskItem = taskQueue.AddTask(task.TaskName);

            return taskItem;
        }
    }
}
