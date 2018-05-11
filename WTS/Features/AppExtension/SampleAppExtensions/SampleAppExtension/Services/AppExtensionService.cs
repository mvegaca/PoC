using System;
using System.Threading.Tasks;
using SampleAppExtension.Activation;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace SampleAppExtension.Services
{
    internal class AppExtensionService : ActivationHandler<BackgroundActivatedEventArgs>
    {
        private bool _appServiceInitialized;
        private BackgroundTaskDeferral _appServiceDeferral;
        private AppServiceConnection _appServiceConnection;

        protected override Task HandleInternalAsync(BackgroundActivatedEventArgs args)
        {
            if (_appServiceInitialized == false)
            {
                _appServiceInitialized = true;

                var taskInstance = args.TaskInstance;
                taskInstance.Canceled += OnCanceled;

                var appService = taskInstance.TriggerDetails as AppServiceTriggerDetails;
                _appServiceDeferral = taskInstance.GetDeferral();
                _appServiceConnection = appService.AppServiceConnection;
                _appServiceConnection.RequestReceived += OnRequestReceived;
                _appServiceConnection.ServiceClosed += OnOnServiceClosed;
            }
            return Task.CompletedTask;
        }

        private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var messageDeferral = args.GetDeferral();
            var response = await ComposeSampleAppServiceResponse(new ExtensionRequest(args.Request));
            await args.Request.SendResponseAsync(response.GetValueSet());
            messageDeferral.Complete();
        }

        private async Task<ExtensionResponse> ComposeSampleAppServiceResponse(ExtensionRequest request)
        {
            var parameter01 = await request.GetParameterAsync<string>("Parameter01");
            var parameter02 = await request.GetParameterAsync<int>("Parameter02");
            var parameter03 = await request.GetParameterAsync<DateTime>("Parameter03");

            var response = new ExtensionResponse();
            await response.AddValueAsync("Response01", $"{parameter01} from Extension");
            await response.AddValueAsync("Response02", parameter02 + 1);
            await response.AddValueAsync("Response03", parameter03.AddDays(1));
            return response;
        }

        private void OnOnServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            _appServiceDeferral.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _appServiceDeferral.Complete();
        }
    }
}
