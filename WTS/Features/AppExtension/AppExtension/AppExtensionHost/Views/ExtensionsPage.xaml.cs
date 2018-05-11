using System;

using AppExtensionHost.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppExtensionHost.Views
{
    public sealed partial class ExtensionsPage : Page
    {
        public ExtensionsViewModel ViewModel { get; } = new ExtensionsViewModel();

        public ExtensionsPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.ExtensionsService.ExtensionsUpdated += OnExtensionsUpdated;
            await ViewModel.InitializeAsync();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            App.ExtensionsService.ExtensionsUpdated -= OnExtensionsUpdated;
        }

        private async void OnExtensionsUpdated(object sender, EventArgs e)
        {
            await ViewModel.InitializeAsync();
        }
    }
}
