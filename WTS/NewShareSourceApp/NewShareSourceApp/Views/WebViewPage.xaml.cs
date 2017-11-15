using System;

using NewShareSourceApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NewShareSourceApp.Views
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
