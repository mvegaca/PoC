using System;
using System.Threading.Tasks;
using Integrated2SuspendAndResumeMockup.Services;

using Windows.ApplicationModel.Activation;

namespace Integrated2SuspendAndResumeMockup.Activation
{
    internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
        private readonly Type _navElement;

        public DefaultActivationHandler(Type navElement)
        {
            _navElement = navElement;
        }

        protected override async Task HandleInternalAsync(IActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter
            if (args is LaunchActivatedEventArgs launchArgs)
            {
                NavigationService.Navigate(_navElement, launchArgs.Arguments);
            }
            else
            {
                NavigationService.Navigate(_navElement);
            }

            await Task.CompletedTask;
        }

        private void Navigate(Type pageType, IActivatedEventArgs args)
        {
            
        }

        protected override bool CanHandleInternal(IActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return NavigationService.Frame.Content == null && _navElement != null;
        }
    }
}
