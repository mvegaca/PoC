using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using ImageGalleryApp.Helpers;
using ImageGalleryApp.Models;
using ImageGalleryApp.Services;

using Windows.Storage;
using Windows.UI.Xaml.Navigation;

namespace ImageGalleryApp.ViewModels
{
    public class ImageGalleryDetailViewModel : Observable
    {
        private object _selectedImage;
        private ObservableCollection<SampleImage> _source;

        public object SelectedImage
        {
            get => _selectedImage;
            set
            {
                Set(ref _selectedImage, value);
                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewModel.ImageGallerySelectedIdKey, ((SampleImage)SelectedImage).ID);
            }
        }

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => Set(ref _source, value);
        }

        public ImageGalleryDetailViewModel()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData();
        }

        public async Task InitializeAsync(SampleImage sampleImage, NavigationMode navigationMode)
        {
            if (sampleImage != null && navigationMode == NavigationMode.New)
            {
                SelectedImage = Source.FirstOrDefault(i => i.ID == sampleImage.ID);
            }
            else
            {
                var selectedImageId = await ApplicationData.Current.LocalSettings.ReadAsync<string>(ImageGalleryViewModel.ImageGallerySelectedIdKey);
                if (!string.IsNullOrEmpty(selectedImageId))
                {
                    SelectedImage = Source.FirstOrDefault(i => i.ID == selectedImageId);
                }
            }
        }
    }
}
