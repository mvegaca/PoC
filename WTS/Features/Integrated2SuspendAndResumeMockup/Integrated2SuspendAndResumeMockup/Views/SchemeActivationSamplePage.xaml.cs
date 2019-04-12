using System;
using System.Collections.Generic;
using Integrated2SuspendAndResumeMockup.Activation;
using Integrated2SuspendAndResumeMockup.Core.Helpers;
using Integrated2SuspendAndResumeMockup.Services;
using Integrated2SuspendAndResumeMockup.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Integrated2SuspendAndResumeMockup.Views
{
    // TODO WTS: Remove this sample page when/if it's not needed.
    // This page is an sample of how to launch a specific page in response to a protocol launch and pass it a value.
    // It is expected that you will delete this page once you have changed the handling of a protocol launch to meet
    // your needs and redirected to another of your pages.
    public sealed partial class SchemeActivationSamplePage : Page
    {
        public SchemeActivationSampleViewModel ViewModel { get; } = new SchemeActivationSampleViewModel();

        public SchemeActivationSamplePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Dictionary<string, string> activationParams)
            {
                ViewModel.Initialize(activationParams);
            }

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
