using System;

using AADSampleApplication.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AADSampleApplication.Views
{
    public sealed partial class LogOutPage : Page
    {
        public LogOutViewModel ViewModel { get; } = new LogOutViewModel();

        public LogOutPage()
        {
            InitializeComponent();
        }
    }
}
