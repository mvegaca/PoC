using System;

using NavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavigationPaneProject.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
