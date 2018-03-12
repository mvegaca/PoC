using System;

using NavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavigationPaneProject.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame);
        }
    }
}
