﻿using System;
using System.Threading.Tasks;

using IntegratedSuspendAndResumeMockup.Services;

using Windows.ApplicationModel.Activation;

namespace IntegratedSuspendAndResumeMockup.Activation
{
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
        // By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args, SuspendAndResumeArgs suspendAndResumeArgs = null)
        {
            // Create data from activation Uri in ProtocolActivatedEventArgs
            var data = new SchemeActivationData(args.Uri);
            if (data.IsValid)
            {
                var activationArgs = new ActivationHandlerArgs()
                {
                    NavigationParameter = data.Parameters,
                    SuspensionState = suspendAndResumeArgs.SuspensionState
                };

                NavigationService.Navigate(data.PageType, activationArgs);
            }
            else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                NavigationService.Navigate(typeof(Views.MainPage));
            }

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(ProtocolActivatedEventArgs args)
        {
            // If your app has multiple handlers of ProtocolActivationEventArgs
            // use this method to determine which to use. (possibly checking args.Uri.Scheme)
            return true;
        }
    }
}