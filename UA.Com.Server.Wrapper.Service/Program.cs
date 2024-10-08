using System.ServiceProcess;

namespace UA.Com.Server.Wrapper.Service
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new OPCUAWrapperService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
