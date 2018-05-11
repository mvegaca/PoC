using System;

using SampleAppExtension.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SampleAppExtension.Views
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
