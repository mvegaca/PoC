using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using WtsBackgroundTransfer.BackgroundTasks;

namespace WtsBackgroundTransfer.Services
{
    public class BackgroundTransferService
    {
        public BackgroundDownloader Downloader { get; private set; }

        public BackgroundTransferService()
        {
            Downloader = new BackgroundDownloader();
        }

        public BackgroundTransferService(BackgroundTransferCompletionGroup completionGroup)
        {
            Downloader = new BackgroundDownloader(completionGroup);
        }

        public void DownloadAsync(Uri uri, IStorageFile resultFile)
        {
            var download = Downloader.CreateDownload(uri, resultFile);
            var startTask = download.StartAsync().AsTask();
            var continueTask = startTask.ContinueWith(OnDownloadCompleted);
            Downloader.CompletionGroup?.Enable();
        }

        private void OnDownloadCompleted(Task<DownloadOperation> task)
        {
        }
    }
}
