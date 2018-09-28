using System;

using NavigationViewWinUI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavigationViewWinUI.Views
{
    public sealed partial class TabbedPage : Page
    {
        public TabbedViewModel ViewModel { get; } = new TabbedViewModel();

        public TabbedPage()
        {
            InitializeComponent();
        }
    }
}
