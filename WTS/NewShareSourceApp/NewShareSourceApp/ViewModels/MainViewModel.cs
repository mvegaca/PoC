using System;

using NewShareSourceApp.Helpers;
using NewShareSourceApp.Services;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using System.Linq;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;

namespace NewShareSourceApp.ViewModels
{
    public class MainViewModel : Observable
    {
        private StorageFile _image;
        private IEnumerable<StorageFile> _files;
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
        }

        private async void OnShare()
        {
            var data = new ShareSourceConfig()
            {
                Title = "My share title",
                Description = "Sharing some content from MyApp"
            };

            if (IncludeText)
            {
                data.SetText("Hello world!");
            }
            if (IncludeWebLink)
            {
                data.SetWebLink(new Uri("https://wwww.microsoft.com/", UriKind.Absolute));
            }
            if (IncludeApplicationLink)
            {
                data.SetApplicationLink(new Uri("new-share-source-app:navigate?page=MainPage"));
            }
            if (IncludeHtml)
            {
                data.SetHtml(GetHtmlSample());
            }
            if (IncludeImage)
            {
                await PickImage();
                if (_image != null)
                {
                    data.SetImage(_image);
                }
            }
            if (IncludeStorageItems)
            {
                await PickMultipleImages();
                if (_files != null && _files.Any())
                {
                    data.SetStorageItems(_files);
                }
            }
            if (IncludeDeferralContent)
            {
                await PickImage();
                if (_image != null)
                {
                    data.SetDeferredContent(StandardDataFormats.Bitmap, GetDeferredDataSample);
                }
            }

            ShareSourceService.ShareData(data);
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

        private async Task PickImage()
        {
            FileOpenPicker imagePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter = { ".jpg", ".png", ".bmp", ".gif", ".tif" }
            };
            _image = await imagePicker.PickSingleFileAsync();
        }

        private async Task PickMultipleImages()
        {
            FileOpenPicker filesPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter = { ".jpg", ".png", ".bmp", ".gif", ".tif" }
            };
            _files = await filesPicker.PickMultipleFilesAsync();
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
