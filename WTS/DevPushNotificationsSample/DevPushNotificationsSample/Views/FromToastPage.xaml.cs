using System;

using DevPushNotificationsSample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DevPushNotificationsSample.Views
{
    public sealed partial class FromToastPage : Page
    {
        public FromToastViewModel ViewModel { get; } = new FromToastViewModel();

        public FromToastPage()
        {
            InitializeComponent();
        }
    }
}
