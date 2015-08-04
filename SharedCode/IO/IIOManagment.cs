#region

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace SharedCode.IO
{
    public interface IIOManagment
    {
        Boolean CheckIfFileExists(String fileName);

        String ReadResourceFileData(Uri file);

        String ReadLocalFileData(String fileName);

        void GetFileStreamFromStorageFolder(
            String fileName,
            StreamType streamType,
            out Stream stream);

        Task<DownloadResult> DownloadFileFromWebToStorageFolder(Uri uriToDownload, 
            string destinationFileName, 
            CancellationToken cToken);

        Task<DownloadResult> DownloadFileFromWebToStorageFolder(string internetFilePath, 
            string destinationFileName, 
            CancellationToken cToken);

        Task CopyFileFromAssetsToStorageFolder(String filePath);

        Stream CreateNewFileInStorageFolder(string fileName);
    }
}
