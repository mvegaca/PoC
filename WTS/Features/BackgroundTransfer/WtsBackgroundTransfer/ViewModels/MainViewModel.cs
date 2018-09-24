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
        //http://ipv4.download.thinkbroadband.com/512MB.zip
        //http://ipv4.download.thinkbroadband.com/10MB.zip
        private readonly Uri _uri = new Uri("http://ipv4.download.thinkbroadband.com/10MB.zip");
        private BackgroundTransferService _backgroundTransferService;
        private ICommand _downloadButton;
        private CoreDispatcher _dispatcher;

        public readonly ObservableCollection<DownloadInfo> Downloads = new ObservableCollection<DownloadInfo>();

        public ICommand DownloadButton => _downloadButton ?? (_downloadButton = new RelayCommand<string>(OnDownload));

        public MainViewModel()
        {
        }

        public async Task InitializeAsync(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            var task = BackgroundTaskService.BackgroundTasks.First(t => t.Match(nameof(CompletionGroupTask))) as IBackgroundTransferBackgroundTask;
            _backgroundTransferService = new BackgroundTransferService("wtsGroup", task);
            //_backgroundTransferService = new BackgroundTransferService("wtsGroup");
            var activeDownloads = await _backgroundTransferService.InitializeAsync();
            if (activeDownloads != null)
            {
                AddDownloadsInfo(activeDownloads);
            }

            _backgroundTransferService.DownloadProgress += UpdateDownloadInfo;
            _backgroundTransferService.DownloadCompleted += UpdateDownloadInfo;
        }

        private async void UpdateDownloadInfo(object sender, DownloadOperation download)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var downloadInfo = Downloads.FirstOrDefault(d => d.FileName == download.ResultFile.Name);
                downloadInfo?.Update(download);
            });
        }

        private async void OnDownload(string parameter)
        {
            var totalFiles = Int32.Parse(parameter);
            var files = new List<(Uri Uri, IStorageFile ResultFile)>();
            for (int i = 0; i < totalFiles; i++)
            {
                var fileName = $"{Path.GetRandomFileName()}.zip";
                var file = await KnownFolders.PicturesLibrary.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
                files.Add((_uri, file));
            }
            var newDownloads = _backgroundTransferService.Download(files);
            AddDownloadsInfo(newDownloads);
        }

        private void AddDownloadsInfo(IEnumerable<DownloadInfo> downloadsInfo)
        {
            foreach (var downloadInfo in downloadsInfo)
            {
                Downloads.Add(downloadInfo);
            }
        }
    }
}
