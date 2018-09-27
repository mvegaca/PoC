using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Core;
using WtsBackgroundTransfer.BackgroundTasks;
using WtsBackgroundTransfer.Helpers;
using WtsBackgroundTransfer.Models;
using WtsBackgroundTransfer.Services;

namespace WtsBackgroundTransfer.ViewModels
{
    public class MainViewModel : Observable
    {
        // http://ipv4.download.thinkbroadband.com/512MB.zip
        // http://ipv4.download.thinkbroadband.com/10MB.zip
        // http://ipv4.download.thinkbroadband.com/5MB.zip
        private readonly Uri _uri = new Uri("http://ipv4.download.thinkbroadband.com/5MB.zip");
        private BackgroundTransferService _backgroundTransferService;
        private ICommand _downloadCommand;
        private RelayCommand _downloadWithTaskCommand;
        private ICommand _pauseCommand;
        private ICommand _resumeCommand;
        private ICommand _clearCommand;
        private CoreDispatcher _dispatcher;

        public readonly ObservableCollection<DownloadInfo> Downloads = new ObservableCollection<DownloadInfo>();

        public ICommand DownloadCommand => _downloadCommand ?? (_downloadCommand = new RelayCommand(OnDownload));

        public RelayCommand DownloadWithTaskCommand => _downloadWithTaskCommand ?? (_downloadWithTaskCommand = new RelayCommand(OnDownloadWithTask, CanDownloadWithTask));

        public ICommand PauseCommand => _pauseCommand ?? (_pauseCommand = new RelayCommand(OnPause));

        public ICommand ResumeCommand => _resumeCommand ?? (_resumeCommand = new RelayCommand(OnResume));

        public ICommand ClearCommand => _clearCommand ?? (_clearCommand = new RelayCommand(OnClear));

        private bool CanDownloadWithTask()
        {
            return !_backgroundTransferService.IsDownloadingWithTask;
        }

        public MainViewModel()
        {
        }

        public async Task InitializeAsync(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _backgroundTransferService = new BackgroundTransferService();
            //_backgroundTransferService = new BackgroundTransferService("wtsGroup");
            var activeDownloads = await _backgroundTransferService.InitializeAsync();
            if (activeDownloads != null)
            {
                AddDownloadsInfo(activeDownloads);
            }

            _backgroundTransferService.DownloadProgress += UpdateDownloadInfo;
            _backgroundTransferService.DownloadCompleted += UpdateDownloadInfo;
            _backgroundTransferService.IsDownloadingWithTaskChanged += OnIsDownloadingWithTaskChanged;
        }

        private async void UpdateDownloadInfo(object sender, DownloadOperation download)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var downloadInfo = Downloads.FirstOrDefault(d => d.FileName == download.ResultFile.Name);
                downloadInfo?.Update(download);
            });
        }

        private async Task DownloadAsync(bool useTask)
        {
            var files = new List<(Uri Uri, IStorageFile ResultFile)>();
            IEnumerable<DownloadInfo> newDownloads;
            for (int i = 0; i < 3; i++)
            {
                var fileName = $"{Path.GetRandomFileName()}.zip";
                var file = await KnownFolders.PicturesLibrary.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
                files.Add((_uri, file));
            }
            if (useTask)
            {
                var completionGroupTask = BackgroundTaskService.BackgroundTasks.First(t => t.Match(nameof(CompletionGroupTask))) as IBackgroundTransferBackgroundTask;
                newDownloads = _backgroundTransferService.Download(files, BackgroundTransferPriority.Default, completionGroupTask);
            }
            else
            {
                newDownloads = _backgroundTransferService.Download(files);
            }
            AddDownloadsInfo(newDownloads);
        }

        private void AddDownloadsInfo(IEnumerable<DownloadInfo> downloadsInfo)
        {
            if (downloadsInfo != null)
            {
                foreach (var downloadInfo in downloadsInfo)
                {
                    Downloads.Add(downloadInfo);
                }
            }
        }

        private async void OnPause()
        {
            await _backgroundTransferService.PauseAllAsync();
        }

        private async void OnResume()
        {
            await _backgroundTransferService.ResumeAllAsync();
        }

        private void OnClear()
        {
            Downloads.RemoveAll(d => d.Status == BackgroundTransferStatus.Completed.ToString());
        }

        private async void OnDownload()
        {
            await DownloadAsync(false);
        }

        private async void OnDownloadWithTask()
        {
            await DownloadAsync(true);
        }

        private void OnIsDownloadingWithTaskChanged(object sender, EventArgs e)
        {
            DownloadWithTaskCommand.OnCanExecuteChanged();
        }
    }
}
