using System;
using System.Collections;
using System.ComponentModel;
using System.ServiceProcess;

namespace WindowsServiceWithWebApi.WinService
{
    [RunInstaller(true)]
    public partial class WinServiceInstaller : System.Configuration.Install.Installer
    {
        public const string SERVICE_NAME = "Simple Windows Service";

        private readonly ServiceProcessInstaller serviceProcessInstaller;
        private readonly ServiceInstaller serviceInstaller;

        public WinServiceInstaller()
        {
            serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = SERVICE_NAME;
            serviceInstaller.Description = "Simple windows servie application.";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DelayedAutoStart = true;

            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);

            InitializeComponent();
        }

        /// <summary>
        /// Stops the service before uninstalling
        /// </summary>
        /// <param name="savedState"></param>
        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            //Stop the service before uninstalling it
            ServiceController _serviceController = new ServiceController(SERVICE_NAME);
            try
            {
                _serviceController.Stop();
            }
            catch (Exception ex)
            {
                //Somthing went wrong while stopping the service
            }
            finally
            {
                _serviceController.Dispose();
                base.OnBeforeUninstall(savedState);
            }
        }
    }
}
