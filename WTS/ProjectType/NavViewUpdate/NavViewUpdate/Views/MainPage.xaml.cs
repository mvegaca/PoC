using System;

using NavViewUpdate.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavViewUpdate.Views
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
