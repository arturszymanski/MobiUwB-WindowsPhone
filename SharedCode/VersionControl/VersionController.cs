#region

using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SharedCode.DataManagment;
using SharedCode.DataManagment.DataAccess;
using SharedCode.IO;
using SharedCode.VersionControl.Models;

#endregion

namespace SharedCode.VersionControl
{
    public class VersionController
    {
        private readonly IIOManagment _ioManager;
        private readonly IReadData _readData;
        private readonly IWriteData _writeData;

        public VersionController(
            IIOManagment ioManager, 
            IReadData readData, 
            IWriteData writeData)
        {
            _ioManager = ioManager;
            _readData = readData;
            _writeData = writeData;
        }

        public async Task<VersioningResult> GetNewestFile(VersioningRequest versioningRequest)
        {
            VersioningResult result = new VersioningResult();
            try
            {
                Boolean deviceFileExists =
                    _ioManager.CheckIfFileExists(
                        versioningRequest.GetDestinationFileName());

                if (deviceFileExists)
                {
                    Boolean isDeviceFileCurrent = await CheckIfFileIsCurrent(versioningRequest);
                    if (!isDeviceFileCurrent)
                    {
                        await DownloadFile(versioningRequest);
                    }
                }
                else
                {
                    await DownloadFile(versioningRequest);
                }

                FillResultWithLocalFile(
                    versioningRequest,
                    result);
            }
            catch (Exception e)
            {
                result.AddException(e);
            }
            return result;
        }

        private async Task DownloadFile(VersioningRequest request)
        {
            DownloadResult downloadInfo = await _ioManager.DownloadFileFromWebToStorageFolder(
                request.GetInternetFile(),
                request.GetDestinationFileName(),
                CancellationToken.None);

            if (!downloadInfo.Succeeded)
            {
                throw downloadInfo.GetExceptions().FirstOrDefault();
            }
        }

        private async Task<Boolean> CheckIfFileIsCurrent(VersioningRequest request)
        {
            DataManager dataManager = new DataManager(_readData, _writeData);
            FileModificationDate fileModificationDate =
                dataManager.RestoreData<FileModificationDate>(
                    request.GetInternetFile());

            DateTime internetFileDateTime = await GetInternetFileModificationDate(request.GetInternetFile());
            if (fileModificationDate.Date < internetFileDateTime)
            {
                return false;
            }
            return true;
        }


        public async Task<DateTime> GetInternetFileModificationDate(String internetFile)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(internetFile);

            DateTime dt;
            using (WebResponse response =
                await Task<WebResponse>.Factory.FromAsync(
                req.BeginGetResponse,
                req.EndGetResponse,
                req))
            {
                String lastModifiedHeaderContent = response.Headers["Last-Modified"];
                if (String.IsNullOrEmpty(lastModifiedHeaderContent))
                {
                    return DateTime.Now;
                }
                dt = DateTime.Parse(lastModifiedHeaderContent);
            }
            return dt;
        }

        private void FillResultWithLocalFile(
            VersioningRequest request,
            VersioningResult result)
        {
            result.SetFileContent(
                _ioManager.ReadLocalFileData(
                request.GetDestinationFileName()));
        }

        private class FileModificationDate : IRestolable<FileModificationDate>
        {
            private DateTime _date;
            public DateTime Date
            {
                get { return _date; }
                set { _date = value; }
            }

            public FileModificationDate()
            {
                _date = DateTime.MinValue;
            }

            public FileModificationDate GetDefaults()
            {
                return new FileModificationDate();
            }
        }
    }
}
