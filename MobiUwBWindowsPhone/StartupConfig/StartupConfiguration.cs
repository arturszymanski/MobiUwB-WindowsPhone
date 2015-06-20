#region

using System;
using System.ComponentModel;
using MobiUwB.StartupConfig.Worker;
using SharedCode.Parsers.Models.ConfigurationXML;
using SharedCode.Parsers.Models.Properties;

#endregion

namespace MobiUwB.StartupConfig
{
    public class StartupConfiguration
    {
        public static String PropertiesFileName = "properties.xml";
        public static String ConfigurationFileName = "configuration.xml";

        public static Properties Properties;
        public static Configuration Configuration;

        public event RunWorkerCompletedEventHandler Finished;

        private readonly StartupBackgroundWorker _startupBackgroundWorker;

        public StartupConfiguration()
        {
            _startupBackgroundWorker = new StartupBackgroundWorker();
            _startupBackgroundWorker.RunWorkerCompleted += OnRunWorkerCompleted; 
        }


        public void startConfiguration()
        {
            _startupBackgroundWorker.RunWorkerAsync();
        }

        private void OnRunWorkerCompleted(
            object sender,
            RunWorkerCompletedEventArgs workerCompletedEventArgs)
        {
            StartupConfigurationResult startupConfigurationResult =
                (StartupConfigurationResult) workerCompletedEventArgs.Result;

            Properties = startupConfigurationResult.Properties;
            Configuration = startupConfigurationResult.Configuration;

            RunWorkerCompletedEventHandler handler = Finished;
            if (handler != null)
            {
                handler(this, workerCompletedEventArgs);
            }
        }
    }
}
