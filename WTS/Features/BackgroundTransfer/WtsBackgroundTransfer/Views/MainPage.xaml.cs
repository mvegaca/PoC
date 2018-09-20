using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WtsBackgroundTransfer.ViewModels;

namespace WtsBackgroundTransfer.Views
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
            ViewModel.Initialize();
        }
    }
}
