
using ImageGalleryApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ImageGalleryApp.Views
{
    public sealed partial class ImageGalleryPage : Page
    {
        public ImageGalleryViewModel ViewModel { get; } = new ImageGalleryViewModel();

        public ImageGalleryPage()
        {
            InitializeComponent();
        }
    }
}
