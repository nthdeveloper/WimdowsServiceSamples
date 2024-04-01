using System;

namespace WindowsServiceWithWorkers.Web.Models
{
    public class SystemInfo
    {
        public string MachineName { get; set; }
        public string OSVersion { get; set; }
        public string UserName { get; set; }
        public long WorkingSet { get; set; }
    }
}
