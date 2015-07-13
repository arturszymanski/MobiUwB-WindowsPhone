using SharedCode.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using Windows.Storage;

namespace NotificationsAgent
{
    public class IoManager : IIOManagment
    {
        public Boolean CheckIfFileExists(String fileName)
        {
            String folder = ApplicationData.Current.LocalFolder.Path;
            String fullSavePath = Path.Combine(
                folder,
                fileName);
            return File.Exists(fullSavePath);
        }

        public Stream CreateNewFileInStorageFolder(
            String fileName)
        {
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;

            Task<StorageFile> file = storageFolder.CreateFileAsync(
                fileName,
                CreationCollisionOption.OpenIfExists).AsTask();
            file.Wait();

            Task<Stream> fileStream = file.Result.OpenStreamForWriteAsync();
            fileStream.Wait();
            return fileStream.Result;
        }


        public String ReadResourceFileData(Uri file)
        {
            StreamResourceInfo strm = Application.GetResourceStream(
                file);
            String fileContents;
            using (StreamReader reader = new StreamReader(strm.Stream))
            {
                fileContents = reader.ReadToEnd();
            }
            return fileContents;
        }

        public String ReadLocalFileData(String fileName)
        {
            String fullSavePath = Path.Combine(
                ApplicationData.Current.LocalFolder.Path,
                fileName);
            String fileContents;

            using (StreamReader reader = new StreamReader(fullSavePath))
            {
                fileContents = reader.ReadToEnd();
            }

            return fileContents;
        }

        public void GetFileStreamFromStorageFolder(
            String fileName,
            StreamType streamType,
            out Stream stream)
        {
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;

            Task<StorageFile> fileTask =
                storageFolder.GetFileAsync(fileName).AsTask();
            fileTask.Wait();
            Task<Stream> streamTask;
            switch (streamType)
            {
                case StreamType.ForRead:
                    {
                        streamTask =
                            fileTask.Result.OpenStreamForReadAsync();
                        break;
                    }
                case StreamType.ForWrite:
                    {
                        streamTask =
                            fileTask.Result.OpenStreamForWriteAsync();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(
                            "Non existing enum value. " +
                            typeof(StreamType));
                    }
            }
            streamTask.Wait();
            stream = streamTask.Result;
        }


        public async Task<DownloadResult> DownloadFileFromWebToStorageFolder(
            Uri uriToDownload,
            String destinationFileName,
            CancellationToken cToken)
        {
            try
            {
                using (Stream mystr = await DownloadFile(uriToDownload))
                {
                    using (Stream stream =
                        CreateNewFileInStorageFolder(
                            destinationFileName))
                    {
                        const int bufferSize = 1024;
                        byte[] buf = new byte[bufferSize];

                        int bytesread = 0;
                        while ((bytesread = await mystr.ReadAsync(buf, 0, bufferSize)) > 0)
                        {
                            cToken.ThrowIfCancellationRequested();
                            stream.Write(buf, 0, bytesread);
                        }
                    }
                }
                return DownloadResult.Succeded;
            }
            catch (OperationCanceledException)
            {
                return DownloadResult.Cancelled;
            }
            catch (Exception e)
            {
                return DownloadResult.Other;
            }
        }


        public async Task<DownloadResult> DownloadFileFromWebToStorageFolder(
            String internetFilePath,
            String destinationFileName,
            CancellationToken cToken)
        {
            Uri dowloadFileUri = new Uri(internetFilePath);
            return await DownloadFileFromWebToStorageFolder(
                dowloadFileUri,
                destinationFileName,
                cToken);
        }



        private Task<Stream> DownloadFile(Uri url)
        {
            var tcs = new TaskCompletionSource<Stream>();
            var wc = new WebClient();
            wc.OpenReadCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    tcs.TrySetException(e.Error);
                }
                else if (e.Cancelled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    tcs.TrySetResult(e.Result);
                }
            };
            wc.OpenReadAsync(url);
            return tcs.Task;
        }

        public async Task CopyFileFromAssetsToStorageFolder(String filePath)
        {
            String assetsPath = Path.Combine("Assets", filePath);
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;

            String fileName = Path.GetFileName(filePath);

            StreamResourceInfo streamResourceInfo =
                Application.GetResourceStream(new Uri(assetsPath, UriKind.Relative));
            using (Stream resourceFileStream = streamResourceInfo.Stream)
            {
                StorageFile newFile = await storageFolder.CreateFileAsync(fileName);
                using (Stream newFileStream = await newFile.OpenStreamForWriteAsync())
                {
                    resourceFileStream.CopyTo(newFileStream);
                }
            }
        }
    }
}
