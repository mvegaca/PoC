using System;

using DevPushNotificationsSample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DevPushNotificationsSample.Views
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
