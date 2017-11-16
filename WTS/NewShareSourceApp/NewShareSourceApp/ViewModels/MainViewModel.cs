using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;

using NewShareSourceApp.Extensions;
using NewShareSourceApp.Helpers;
using NewShareSourceApp.Models;
using NewShareSourceApp.Services;

namespace NewShareSourceApp.ViewModels
{
    public class MainViewModel : Observable
    {
        private DataTransferManager _dataTransferManager;
        private StorageFile _image;
        private IReadOnlyList<StorageFile> _files;
        private RelayCommand _shareCommand;

        public RelayCommand ShareCommand => _shareCommand ?? (_shareCommand = new RelayCommand(OnShare, CanShare));

        private bool CanShare()
        {
            return IncludeText || IncludeWebLink || IncludeApplicationLink || IncludeHtml || IncludeImage || IncludeStorageItems || IncludeDeferralContent;
        }

        private bool _includeText = true;
        public bool IncludeText
        {
            get => _includeText;
            set
            {
                Set(ref _includeText, value);
                ShareCommand.OnCanExecuteChanged();
            }
        }

        private bool _includeWebLink = true;
        public bool IncludeWebLink
        {
            get => _includeWebLink;
            set
            {
                Set(ref _includeWebLink, value);
                ShareCommand.OnCanExecuteChanged();
            }
        }

        private bool _includeApplicationLink;
        public bool IncludeApplicationLink
        {
            get => _includeApplicationLink;
            set
            {
                Set(ref _includeApplicationLink, value);
                ShareCommand.OnCanExecuteChanged();
            }
        }

        private bool _includeHtml;
        public bool IncludeHtml
        {
            get => _includeHtml;
            set
            {
                Set(ref _includeHtml, value);
                ShareCommand.OnCanExecuteChanged();
            }
        }

        private bool _includeImage;
        public bool IncludeImage
        {
            get => _includeImage;
            set
            {
                Set(ref _includeImage, value);
                ShareCommand.OnCanExecuteChanged();
            }
        }

        private bool _includeStorageItems;
        public bool IncludeStorageItems
        {
            get => _includeStorageItems;
            set
            {
                Set(ref _includeStorageItems, value);
                ShareCommand.OnCanExecuteChanged();
            }
        }

        private bool _includeDeferralContent;
        public bool IncludeDeferralContent
        {
            get => _includeDeferralContent;
            set
            {
                Set(ref _includeDeferralContent, value);
                ShareCommand.OnCanExecuteChanged();
            }
        }


        public MainViewModel()
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
        }

        public void RegisterEvents()
        {
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        public void UnregisterEvents()
        {
            _dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // This event will be fired when the share operation starts.
            // We need to add data to DataRequestedEventArgs through SetData extension method
            ShareSourceData config = new ShareSourceData("My share title", "My share description");

            // TODO WTS: Use ShareSourceConfig instance to set the data you want to share
            ConfigureDataToShare(config); //For the poc we have added this method. the user would select the data and set into config under his preference

            args.Request.SetData(config);
            args.Request.Data.ShareCompleted += OnShareCompleted;

        }

        private void OnShareCompleted(DataPackage sender, ShareCompletedEventArgs args)
        {
            // This event will be fired when Share Operation will finish
            // TODO WTS: If you need to handle any action when de data is shared implement on this method
        }

        private void ConfigureDataToShare(ShareSourceData config)
        {
            if (IncludeText)
            {
                config.SetText("Hello world!");
            }
            if (IncludeWebLink)
            {
                config.SetWebLink(new Uri("https://wwww.microsoft.com/", UriKind.Absolute));
            }
            if (IncludeApplicationLink)
            {
                config.SetApplicationLink(new Uri("new-share-source-app:navigate?page=MainPage"));
            }
            if (IncludeHtml)
            {
                config.SetHtml(GetHtmlSample());
            }
            if (IncludeImage && _image != null)
            {
                config.SetImage(_image);
            }
            if (IncludeStorageItems && _files != null && _files.Any())
            {
                config.SetStorageItems(_files);
            }
            if (IncludeDeferralContent && _image != null)
            {
                config.SetDeferredContent(StandardDataFormats.Bitmap, GetDeferredDataSample);
            }
        }

        private async void OnShare()
        {
            if ((IncludeImage || IncludeDeferralContent) && _image == null)
            {
                _image = await FileOpenPickerService.PickImage();
            }
            if (IncludeStorageItems && (_files == null || !_files.Any()))
            {
                _files = await FileOpenPickerService.PickMultipleImages();
            }
            DataTransferManager.ShowShareUI();
        }

        private string GetHtmlSample()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<body>");
            sb.AppendLine("<p>");
            sb.AppendLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
            sb.AppendLine("</p>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }

        private async Task<object> GetDeferredDataSample()
        {
            // Decode the image and re-encode it at 50% width and height.
            InMemoryRandomAccessStream inMemoryStream = new InMemoryRandomAccessStream();
            IRandomAccessStream imageStream = await _image.OpenAsync(FileAccessMode.Read);
            BitmapDecoder imageDecoder = await BitmapDecoder.CreateAsync(imageStream);
            BitmapEncoder imageEncoder = await BitmapEncoder.CreateForTranscodingAsync(inMemoryStream, imageDecoder);
            imageEncoder.BitmapTransform.ScaledWidth = (uint)(imageDecoder.OrientedPixelWidth * 0.5);
            imageEncoder.BitmapTransform.ScaledHeight = (uint)(imageDecoder.OrientedPixelHeight * 0.5);
            await imageEncoder.FlushAsync();
            return RandomAccessStreamReference.CreateFromStream(inMemoryStream);
        }
    }
}
