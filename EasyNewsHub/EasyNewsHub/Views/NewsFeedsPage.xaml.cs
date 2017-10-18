using System;

using EasyNewsHub.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EasyNewsHub.Views
{
    public sealed partial class NewsFeedsPage : Page
    {
        public NewsFeedsViewModel ViewModel { get; } = new NewsFeedsViewModel();

        public NewsFeedsPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadAsync();
        }
    }
}
