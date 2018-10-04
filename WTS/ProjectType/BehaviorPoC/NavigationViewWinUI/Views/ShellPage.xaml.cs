using System;
using NavigationViewWinUI.ViewModels;
using Windows.UI.Xaml.Controls;

namespace NavigationViewWinUI.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, winUiNavigationView, KeyboardAccelerators);
        }
    }
}
