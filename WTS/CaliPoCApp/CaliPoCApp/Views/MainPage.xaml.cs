using System;

using CaliPoCApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace CaliPoCApp.Views
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
