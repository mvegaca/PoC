using System;

using NavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavigationPaneProject.Views
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
