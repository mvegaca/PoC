using System;

using IntegratedSuspendAndResumeMockup.ViewModels;

using Windows.UI.Xaml.Controls;

namespace IntegratedSuspendAndResumeMockup.Views
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
