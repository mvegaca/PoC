using System;

using EasyNewsHub.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace EasyNewsHub.Views
{
    public sealed partial class NewsListPage : Page
    {
        public NewsListViewModel ViewModel { get; } = new NewsListViewModel();

        public NewsListPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var feedId = e.Parameter.ToString();
            await ViewModel.LoadAsync(feedId);
        }
    }
}
