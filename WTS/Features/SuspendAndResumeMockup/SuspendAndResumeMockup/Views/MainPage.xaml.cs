using System;

using SuspendAndResumeMockup.ViewModels;

using Windows.UI.Xaml.Controls;

namespace SuspendAndResumeMockup.Views
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
