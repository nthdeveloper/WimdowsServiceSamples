using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceWithWebUI.Web.Models
{
    public class SystemInfo
    {
        public string MachineName { get; set; }
        public string OSVersion { get; set; }
        public string UserName { get; set; }
        public long WorkingSet { get; set; }
    }
}
