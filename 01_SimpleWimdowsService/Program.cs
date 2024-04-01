using System;
using System.Reflection;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Configuration.Install;
using SimpleWimdowsService.WinService;

namespace SimpleWimdowsService
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (Environment.UserInteractive)//Setup is executing or running in debug mode as an interactive console application
            {
                List<string> parameters = new List<string>(args);

                if (parameters.Contains("--install"))//Installer is installing the service
                {
                    parameters.Remove("--install");//Remove the extra parameter that we added just to know the mode
                    parameters.Add(Assembly.GetExecutingAssembly().Location);//Add currently executing application's full path

                    try
                    {
                        //Install (register) Windows service using ManagedInstallerClass
                        ManagedInstallerClass.InstallHelper(parameters.ToArray());
                    }
                    catch (Exception e)
                    {
                        //Something went wrong while registring the service
                        return -1;
                    }
                }
                else if (parameters.Contains("--uninstall"))//Installer is uninstalling the service
                {
                    parameters.Remove("--uninstall");//Remove the extra parameter that we added just to know the mode
                    parameters.Insert(0, "/u");//Tell ManagedInstallerClass to *unregister* the service
                    parameters.Add(Assembly.GetExecutingAssembly().Location);//Add currently executing application's full path

                    try
                    {
                        ManagedInstallerClass.InstallHelper(parameters.ToArray());
                    }
                    catch (Exception e)
                    {
                        //Something went wrong while unregistering the service
                        return -1;
                    }
                }
                else//Running in debug mode as an interactive console application
                {
                    var myService = new SampleService();
                    myService.Start();

                    Console.WriteLine("Press enter to stop.");
                    Console.ReadLine();

                    myService.Stop();
                }
            }
            else//User is not interactive (running as a Windows service in the background)
            {
                //Set the working directory as the application's directory
                if(Environment.CurrentDirectory != AppDomain.CurrentDomain.BaseDirectory)
                    Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

                //Constrcut the service object that contains actual operations (it is a regular class, nothing special)
                var myService = new SampleService();

                //Create the Windows service host that will receive Start, Stop, Pause... commands from operating system
                ServiceBase[] servicesToRun = new ServiceBase[] { new WinServiceHost(myService) };

                //Run the created host objects (single host most of the time)
                ServiceBase.Run(servicesToRun);
            }

            return 0;
        }
    }
}
