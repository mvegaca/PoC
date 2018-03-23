using Caliburn.Micro;

using CaliPoCApp.Services;
using CaliPoCApp.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CaliPoCApp.Views
{
    public sealed partial class ShellPage : IShellView
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public ShellPage()
        {
            InitializeComponent();
        }

        public INavigationService CreateNavigationService(WinRTContainer container)
        {
            return container.RegisterNavigationService(shellFrame);
        }

        private void OnStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ViewModel.StateChanged(e);
        }
    }
}
