using System;
using System.IO;
using System.ServiceProcess;
using Opc.Ua;
using Opc.Ua.Com.Client;
using Opc.Ua.Configuration;

namespace UA.Com.Server.Wrapper.Service
{
    public partial class OPCUAWrapperService : ServiceBase
    {
        ApplicationInstance application = new ApplicationInstance();

        public OPCUAWrapperService()
        {
            InitializeComponent();

            application.ApplicationName = "UA COM Server Wrapper";
            application.ApplicationType = ApplicationType.Server;
            application.ConfigSectionName = "Opc.Ua.ComServerWrapper";
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile($"Service is started at {DateTime.Now}");

            try
            {
                string fileConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Opc.Ua.ComServerWrapper.Config.xml");

                // load the application configuration.
                application.LoadApplicationConfiguration(fileConfigPath, false).Wait();

                // check the application certificate.
                application.CheckApplicationInstanceCertificate(false, 0).Wait();

                // start the server.
                application.Start(new ComWrapperServer()).Wait();
            }
            catch (Exception ex)
            {
                WriteToFile(ex.ToString());
            }
        }

        protected override void OnStop()
        {
            WriteToFile($"Service is stopped at {DateTime.Now}");
        }

        public void WriteToFile(string message)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"logs\\servicelog{DateTime.Now.Date.ToShortDateString().Replace('/', '_')}.txt");

            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(message);
                }
            }
        }
    }
}
