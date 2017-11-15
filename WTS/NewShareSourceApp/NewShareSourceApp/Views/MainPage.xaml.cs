using System;

using NewShareSourceApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NewShareSourceApp.Views
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
