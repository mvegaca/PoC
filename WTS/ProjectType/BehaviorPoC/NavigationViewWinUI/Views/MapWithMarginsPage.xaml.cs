using System;

using NavigationViewWinUI.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NavigationViewWinUI.Views
{
    public sealed partial class MapWithMarginsPage : Page
    {
        public MapWithMarginsViewModel ViewModel { get; } = new MapWithMarginsViewModel();

        public MapWithMarginsPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync(mapControl);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Cleanup();
        }
    }
}
