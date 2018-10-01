using System;

using NavigationViewWinUI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavigationViewWinUI.Views
{
    public sealed partial class WebViewPage : Page
    {
        public WebViewViewModel ViewModel { get; } = new WebViewViewModel();

        public WebViewPage()
        {
            InitializeComponent();
            ViewModel.Initialize(webView);
        }
    }
}
