using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using log4net;
using WindowsServiceWithWebUI.Web.Models;

namespace WindowsServiceWithWebUI.Web.Controllers
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
    }
}
