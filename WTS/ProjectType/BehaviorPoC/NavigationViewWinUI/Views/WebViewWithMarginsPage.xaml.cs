using System;

using NavigationViewWinUI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavigationViewWinUI.Views
{
    public sealed partial class WebViewWithMarginsPage : Page
    {
        public WebViewWithMarginsViewModel ViewModel { get; } = new WebViewWithMarginsViewModel();

        public WebViewWithMarginsPage()
        {
            InitializeComponent();
            ViewModel.Initialize(webView);
        }
    }
}
