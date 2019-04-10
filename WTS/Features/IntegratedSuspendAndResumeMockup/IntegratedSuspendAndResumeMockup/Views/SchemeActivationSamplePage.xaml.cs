using System;
using System.Collections.Generic;
using IntegratedSuspendAndResumeMockup.Activation;
using IntegratedSuspendAndResumeMockup.Core.Helpers;
using IntegratedSuspendAndResumeMockup.Services;
using IntegratedSuspendAndResumeMockup.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IntegratedSuspendAndResumeMockup.Views
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
            if (e.Parameter is ActivationHandlerArgs args)
            {
                if (args.NavigationParameter is Dictionary<string, string> activationParams)
                {
                    ViewModel.Initialize(activationParams);
                }

                if (args.SuspensionState != null)
                {
                    ViewModel.Text = args.SuspensionState.Data.ToString();
                }
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
