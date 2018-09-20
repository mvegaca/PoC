using System;
using System.Linq;
using System.Windows.Input;
using WtsBackgroundTransfer.BackgroundTasks;
using WtsBackgroundTransfer.Helpers;
using WtsBackgroundTransfer.Services;

namespace WtsBackgroundTransfer.ViewModels
{
    public class MainViewModel : Observable
    {
        private readonly Uri _uri = new Uri("http://ipv4.download.thinkbroadband.com/10MB.zip");
        private const string _fileName = "10MB.zip";
        private BackgroundTransferService _backgroundTransferService;

        private ICommand _downloadButton;

        public ICommand DownloadButton => _downloadButton ?? (_downloadButton = new RelayCommand(OnDownload));

        public MainViewModel()
        {
        }

        public void Initialize()
        {
            var task = BackgroundTaskService.BackgroundTasks.First(t => t.Match(nameof(CompletionGroupTask))) as CompletionGroupTask;
            _backgroundTransferService = new BackgroundTransferService(task.CompletionGroup);
        }

        private async void OnDownload()
        {
            var file = await FileHelper.GetFromPicturesLibraryAsync(_fileName);
            _backgroundTransferService.DownloadAsync(_uri, file);
        }
    }
}
