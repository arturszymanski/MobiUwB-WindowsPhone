#region

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using MobiUwB.DataAccess;
using MobiUwB.Utilities;
using SharedCode.VersionControl;
using SharedCode.VersionControl.Models;
using SharedCode.Parsers;
using MobiUwB.IO;

#endregion

namespace MobiUwB.StartupConfig.Worker
{
    public class StartupBackgroundWorker : BackgroundWorker
    {
        private StartupConfigurationResult _result;

        private String _readWriteXmlPath;
        private String _configurationXmlPath;

        public StartupBackgroundWorker()
        {
            SetEvents();
        }

        private void SetEvents()
        {
            DoWork += StartupBackgroundWorker_DoWork;
        }

        private void OnStartupBackgroundWorker_DoWorkStart(DoWorkEventArgs e)
        {
            _result = new StartupConfigurationResult();
            e.Result = _result;
        }

        void StartupBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            OnStartupBackgroundWorker_DoWorkStart(e);
            try
            {
                PrepareFilePaths();
                Task propTask = DeserializePropertiesXml(_result);
                propTask.Wait();
                Task configTask = DeserializeConfigurationXml(_result);
                configTask.Wait();
            }
            catch (Exception exception)
            {
                _result.AddError(exception);
            }
            OnStartupBackgroundWorker_DoWorkEnd(e);
        }

        private void OnStartupBackgroundWorker_DoWorkEnd(DoWorkEventArgs e)
        {
            foreach (string error in _result.GetErrors())
            {
                Debug.WriteLine(error);
            }
        }

        private async Task DeserializeConfigurationXml(StartupConfigurationResult result)
        {
            ReadData readData = new ReadData();
            WriteData writeData = new WriteData();

            VersionController versionController = 
                new VersionController(
                    Globals.IoManager, 
                    readData, 
                    writeData);

            VersioningRequest versioningRequest =
                new VersioningRequest(
                    result.Properties.Configuration.Path, StartupConfiguration.ConfigurationFileName);

            VersioningResult versioningResult =
                await versionController.GetNewestFile(versioningRequest);

            if (versioningResult.Succeeded)
            {
                App.XmlParser.Deserialize(_configurationXmlPath, out result.Configuration);
            }
            else
            {
                result.AddErrorMessages(versioningResult.GetErrors());
            }
        }

        private void PrepareFilePaths()
        {
            _readWriteXmlPath = Path.Combine(
                ApplicationData.Current.LocalFolder.Path, StartupConfiguration.PropertiesFileName);


            _configurationXmlPath = Path.Combine(
                ApplicationData.Current.LocalFolder.Path, StartupConfiguration.ConfigurationFileName);
        }

        private async Task DeserializePropertiesXml(StartupConfigurationResult result)
        {
            String fileName = StartupConfiguration.PropertiesFileName;
            if (!Globals.IoManager.CheckIfFileExists(_readWriteXmlPath))
            {
                await Globals.IoManager.CopyFileFromAssetsToStorageFolder(fileName);
            }
            App.XmlParser.Deserialize(fileName,
                out result.Properties);
        }
    }
}
