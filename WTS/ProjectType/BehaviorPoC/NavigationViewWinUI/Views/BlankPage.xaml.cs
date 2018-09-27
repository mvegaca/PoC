using System;

using NavigationViewWinUI.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavigationViewWinUI.Views
{
    public sealed partial class BlankPage : Page
    {
        public BlankViewModel ViewModel { get; } = new BlankViewModel();

        public BlankPage()
        {
            InitializeComponent();
        }
    }
}
