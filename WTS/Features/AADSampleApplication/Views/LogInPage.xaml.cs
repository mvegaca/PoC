using System;

using AADSampleApplication.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AADSampleApplication.Views
{
    public sealed partial class LogInPage : Page
    {
        public LogInViewModel ViewModel { get; } = new LogInViewModel();

        public LogInPage()
        {
            InitializeComponent();
        }
    }
}
