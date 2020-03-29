using System.Reflection;

namespace MyService
{
    public static class ServiceConfiguration
    {
        public static readonly string ServiceName 
                                = "MyService";
        public static readonly string ExecutingAssebmlyLocation 
                                = Assembly.GetExecutingAssembly().Location;
        public static readonly string UrlPort 
                                = "8000";
    }
}
