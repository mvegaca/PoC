using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using NewShareSourceApp.ViewModels;

namespace NewShareSourceApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.RegisterEvents();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.UnregisterEvents();
        }
    }
}
