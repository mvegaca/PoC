using AADSampleApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace AADSampleApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
