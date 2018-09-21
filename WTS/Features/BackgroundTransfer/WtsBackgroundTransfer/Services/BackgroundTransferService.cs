using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using WtsBackgroundTransfer.BackgroundTasks;
using WtsBackgroundTransfer.Models;

namespace WtsBackgroundTransfer.Services
{
    public class BackgroundTransferService
    {
        private readonly BackgroundTransferGroup _group;
        private readonly IBackgroundTransferBackgroundTask _backgroundTask;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public event EventHandler<DownloadOperation> DownloadProgress;
        public event EventHandler<DownloadOperation> DownloadCompleted;

        public BackgroundTransferService(string groupName)
        {
            _group = BackgroundTransferGroup.CreateGroup(groupName);
        }

        public BackgroundTransferService(string groupName, IBackgroundTransferBackgroundTask backgroundTask)
        {
            _group = BackgroundTransferGroup.CreateGroup(groupName);
            _backgroundTask = backgroundTask;
        }

        public async Task<IEnumerable<DownloadInfo>> InitializeAsync()
        {
            try
            {
                var downloads = await BackgroundDownloader.GetCurrentDownloadsForTransferGroupAsync(_group);
                if (downloads.Any())
                {
                    foreach (var download in downloads)
                    {
                        HandleDownload(download, false);
                    }
                    return downloads.Select(d => new DownloadInfo(d));
                }
            }
            catch (Exception)
            {
            }

            return null;
        }        

        public DownloadInfo Download(Uri uri, IStorageFile resultFile, BackgroundTransferPriority priority = BackgroundTransferPriority.Default)
        {
            var downloader = GetDownloader();
            var download = downloader.CreateDownload(uri, resultFile);
            download.Priority = priority;
            downloader.CompletionGroup?.Enable();
            HandleDownload(download, true);
            var downloadInfo = new DownloadInfo(download);
            return downloadInfo;
        }

        private BackgroundDownloader GetDownloader()
        {
            if (_backgroundTask != null)
            {
                // Register a new BackgroundTask with new CompletionGroup
                var completionGroup = _backgroundTask.GetCompletionGroup();
                return new BackgroundDownloader(completionGroup)
                {
                    TransferGroup = _group
                };
            }
            else
            {
                return new BackgroundDownloader()
                {
                    TransferGroup = _group
                };
            }
        }

        private void HandleDownload(DownloadOperation download, bool start)
        {
            try
            {
                var callback = new Progress<DownloadOperation>(OnDownloadProgress);
                if (start)
                {
                    download.StartAsync()
                            .AsTask(_cts.Token, callback)
                            .ContinueWith(OnDownloadCompleted);
                }
                else
                {
                    download.AttachAsync()
                            .AsTask(_cts.Token, callback)
                            .ContinueWith(OnDownloadCompleted);
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception)
            {
            }
            finally
            {
            }
        }

        private async void OnDownloadCompleted(Task<DownloadOperation> task)
        {
            var download = await task;
            DownloadCompleted?.Invoke(this, download);
        }

        private void OnDownloadProgress(DownloadOperation download)
        {
            DownloadProgress?.Invoke(this, download);
        }
    }
}
