﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IntegratedSuspendAndResumeMockup.Activation;
using IntegratedSuspendAndResumeMockup.Core.Helpers;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IntegratedSuspendAndResumeMockup.Services
{
    // For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md
    internal class ActivationService
    {
        private readonly App _app;
        private readonly Type _defaultNavItem;
        private Lazy<UIElement> _shell;

        private object _lastActivationArgs;

        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _app = app;
            _shell = shell;
            _defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize things like registering background task before the app is loaded
                await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    Window.Current.Content = _shell?.Value ?? new Frame();
                }
            }

            await HandleActivationAsync(activationArgs);
            _lastActivationArgs = activationArgs;

            if (IsInteractive(activationArgs))
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync();
            }
        }

        private async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        private async Task<SuspendAndResumeArgs> GetSuspendAndResumeArgsAsync(IActivatedEventArgs activationArgs)
        {
            if (activationArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                return await Singleton<SuspendAndResumeService>.Instance.GetSuspendAndResumeArgs();
            }

            return null;
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var suspendAndResumeArgs = await GetSuspendAndResumeArgsAsync(activationArgs as IActivatedEventArgs);

            var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));
            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs, suspendAndResumeArgs);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs, suspendAndResumeArgs);
                }
            }
        }

        private async Task StartupAsync()
        {
            // TODO WTS: This is a sample to demonstrate how to add a UserActivity. Please adapt and move this method call to where you consider convenient in your app.
            await UserActivityService.AddSampleUserActivity();
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<SchemeActivationHandler>.Instance;
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }
    }
}