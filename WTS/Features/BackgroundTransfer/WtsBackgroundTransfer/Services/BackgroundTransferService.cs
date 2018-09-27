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
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private List<Task> _downloadTasks = new List<Task>();

        public event EventHandler<DownloadOperation> DownloadProgress;
        public event EventHandler<DownloadOperation> DownloadCompleted;
        public event EventHandler IsDownloadingWithTaskChanged;
        public bool IsDownloadingWithTask;

        public BackgroundTransferService()
        {
        }

        public BackgroundTransferService(string groupName)
        {
            _group = BackgroundTransferGroup.CreateGroup(groupName);
        }

        public async Task<IEnumerable<DownloadInfo>> InitializeAsync()
        {
            try
            {
                var downloads = await GetActiveDownloadsAsync();
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

        public IEnumerable<DownloadInfo> Download(IEnumerable<(Uri Uri, IStorageFile ResultFile)> files, BackgroundTransferPriority priority = BackgroundTransferPriority.Default, IBackgroundTransferBackgroundTask backgroundTask = null)
        {
            if (backgroundTask != null)
            {
                if (!IsDownloadingWithTask)
                {
                    IsDownloadingWithTask = true;
                    IsDownloadingWithTaskChanged?.Invoke(this, EventArgs.Empty);
                    _downloadTasks.Clear();
                }
                else
                {
                    return null;
                }
            }
            var downloadsInfo = new List<DownloadInfo>();
            var downloader = GetDownloader(backgroundTask);
            foreach (var file in files)
            {
                var download = downloader.CreateDownload(file.Uri, file.ResultFile);
                download.Priority = priority;
                HandleDownload(download, true);
                var downloadInfo = new DownloadInfo(download);
                downloadsInfo.Add(downloadInfo);
            }
            downloader.CompletionGroup?.Enable();
            Task.WhenAll(_downloadTasks).ContinueWith((task) =>
            {
                if (backgroundTask != null)
                {
                    IsDownloadingWithTask = false;
                    IsDownloadingWithTaskChanged?.Invoke(this, EventArgs.Empty);
                }
            });
            return downloadsInfo;
        }

        public async Task PauseAllAsync()
        {
            var downloads = await GetActiveDownloadsAsync();
            var runningDownloads = downloads.Where(d => d.Progress.Status == BackgroundTransferStatus.Running);
            runningDownloads.ToList().ForEach(d => d.Pause());
        }

        public async Task ResumeAllAsync()
        {
            var downloads = await GetActiveDownloadsAsync();
            var runningDownloads = downloads.Where(d => d.Progress.Status == BackgroundTransferStatus.PausedByApplication);
            runningDownloads.ToList().ForEach(d => d.Resume());
        }

        private async Task<IReadOnlyList<DownloadOperation>> GetActiveDownloadsAsync()
        {
            if (_group == null)
            {
                return await BackgroundDownloader.GetCurrentDownloadsAsync();
            }
            else
            {
                return await BackgroundDownloader.GetCurrentDownloadsForTransferGroupAsync(_group);
            }
        }

        private BackgroundDownloader GetDownloader(IBackgroundTransferBackgroundTask backgroundTask = null)
        {
            var downloader = backgroundTask == null
                                ? new BackgroundDownloader()
                                : new BackgroundDownloader(backgroundTask.GetCompletionGroup());
            if (_group != null)
            {
                downloader.TransferGroup = _group;
            }

            return downloader;
        }

        private void HandleDownload(DownloadOperation download, bool start)
        {
            try
            {
                var callback = new Progress<DownloadOperation>(OnDownloadProgress);
                Task downloadTask;
                if (start)
                {
                    downloadTask = download.StartAsync()
                            .AsTask(_cts.Token, callback)
                            .ContinueWith(OnDownloadCompleted);
                }
                else
                {
                    downloadTask = download.AttachAsync()
                            .AsTask(_cts.Token, callback)
                            .ContinueWith(OnDownloadCompleted);
                }
                _downloadTasks.Add(downloadTask);
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
