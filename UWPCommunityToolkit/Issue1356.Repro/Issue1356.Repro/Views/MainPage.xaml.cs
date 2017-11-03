using System;

using Issue1356.Repro.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Issue1356.Repro.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }
    }
}
