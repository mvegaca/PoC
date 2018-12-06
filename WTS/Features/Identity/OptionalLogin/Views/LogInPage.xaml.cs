using System;

using OptionalLogin.ViewModels;

using Windows.UI.Xaml.Controls;

namespace OptionalLogin.Views
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
