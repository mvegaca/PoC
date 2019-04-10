using System;
using IntegratedSuspendAndResumeMockup.Core.Helpers;
using IntegratedSuspendAndResumeMockup.Services;
using IntegratedSuspendAndResumeMockup.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IntegratedSuspendAndResumeMockup.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var suspensionState = e.Parameter as SuspensionState;
            if (suspensionState != null)
            {
                ViewModel.Text = suspensionState.Data.ToString();
            }
            Singleton<SuspendAndResumeService>.Instance.OnBackgroundEntering += OnBackgroundEntering;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Singleton<SuspendAndResumeService>.Instance.OnBackgroundEntering -= OnBackgroundEntering;
        }

        public void OnBackgroundEntering(object sender, SuspendAndResumeArgs e)
        {
            e.SuspensionState.Data = ViewModel.Text;
        }
    }
}
