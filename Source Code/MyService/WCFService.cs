using System.ServiceModel;

namespace MyService
{
    [ServiceContract(Namespace = "http://MyService")]
    public interface IWCFService
    {
        [OperationContract]
        string Echo(string text);
        [OperationContract]
        string GetServiceName();
    }

    public class WCFService : IWCFService
    {
        public string Echo(string text)
        {
            return text;
        }

        public string GetServiceName()
        {
            return ServiceConfiguration.ServiceName;
        }
    }
}
