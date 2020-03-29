using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;

namespace MyService
{
    public class WService : ServiceBase
    {
        public ServiceHost serviceHost = null;
        public WService()
        {
            ServiceName = ServiceConfiguration.ServiceName;
        }

        public static void Main(string[] args)
        {
            if (args != null && args.Length != 0 && args[0].Length >= 1 && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    default:
                        break;
                    case "i":
                        ProjectInstaller.Install();
                        break;
                    case "u":
                        ProjectInstaller.Uninstall();
                        break;
                }
            }
            else
            {
                ServiceBase.Run(new WService());
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (serviceHost != null)
                    serviceHost.Close();

                Uri httpBaseAddress = new Uri($"http://{System.Net.Dns.GetHostName()}:{ServiceConfiguration.UrlPort}/{ServiceConfiguration.ServiceName}/service");

                serviceHost = new ServiceHost(typeof(WCFService), httpBaseAddress);

                WSHttpBinding wsHttpBinding = new WSHttpBinding();
                wsHttpBinding.MaxReceivedMessageSize = 70000;
                wsHttpBinding.MessageEncoding = WSMessageEncoding.Text;

                serviceHost.AddServiceEndpoint(typeof(IWCFService), wsHttpBinding, "");
                serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = true });

                serviceHost.Open();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.ToString());
            }
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
