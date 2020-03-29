using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace MyService
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public ProjectInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = ServiceConfiguration.ServiceName;
            service.StartType = ServiceStartMode.Automatic;
            Installers.Add(process);
            Installers.Add(service);
        }

        public static bool Install()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { ServiceConfiguration.ExecutingAssebmlyLocation });
                StartService(ServiceConfiguration.ServiceName, 7000);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool Uninstall()
        {
            StopService(ServiceConfiguration.ServiceName, 7000);
            try
            {
                ManagedInstallerClass.InstallHelper(new string[] { "/u", ServiceConfiguration.ExecutingAssebmlyLocation });
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(timeoutMilliseconds));
            }
            catch (Exception ex)
            {
                //....
            }
        }

        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(timeoutMilliseconds));
            }
            catch (Exception ex)
            {
                //....
            }
        }
    }
}
