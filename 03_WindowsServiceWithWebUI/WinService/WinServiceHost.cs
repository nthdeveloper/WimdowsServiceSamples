using System;
using System.ServiceProcess;

namespace WindowsServiceWithWebUI.WinService
{
    partial class WinServiceHost : ServiceBase
    {
        readonly SampleService _service;

        public WinServiceHost(SampleService service)
        {
            InitializeComponent();
            _service = service;
        }

        protected override void OnStart(string[] args)
        {
            _service.Start();
        }

        protected override void OnPause()
        {
            _service.Pause();
        }

        protected override void OnContinue()
        {
            _service.Resume();
        }

        protected override void OnStop()
        {
            _service.Stop();
        }        
    }
}
