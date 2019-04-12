using System;
using Integrated2SuspendAndResumeMockup.Core.Helpers;
using Integrated2SuspendAndResumeMockup.Services;
using Integrated2SuspendAndResumeMockup.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Integrated2SuspendAndResumeMockup.Views
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
            Singleton<SuspendAndResumeService>.Instance.OnBackgroundEntering += OnBackgroundEntering;
            Singleton<SuspendAndResumeService>.Instance.OnDataRestored += OnDataRestored;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Singleton<SuspendAndResumeService>.Instance.OnBackgroundEntering -= OnBackgroundEntering;
            Singleton<SuspendAndResumeService>.Instance.OnDataRestored -= OnDataRestored;
        }

        private void OnDataRestored(object sender, SuspendAndResumeArgs suspendAndResumeArgs)
        {
            var suspensionState = suspendAndResumeArgs.SuspensionState;
            ViewModel.Text = suspensionState.Data.ToString();
        }

        public void OnBackgroundEntering(object sender, SuspendAndResumeArgs e)
        {
            e.SuspensionState.Data = ViewModel.Text;
        }
    }
}
