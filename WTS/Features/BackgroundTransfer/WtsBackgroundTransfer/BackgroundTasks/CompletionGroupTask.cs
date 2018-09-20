using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel.Background;
using Windows.Networking.BackgroundTransfer;
using Windows.System.Threading;
using Windows.UI.Notifications;
using WtsBackgroundTransfer.Helpers;
using WtsBackgroundTransfer.Services;

namespace WtsBackgroundTransfer.BackgroundTasks
{
    public sealed class CompletionGroupTask : BackgroundTask
    {
        public BackgroundTransferCompletionGroup CompletionGroup = new BackgroundTransferCompletionGroup();
        public static string Message { get; set; }

        private volatile bool _cancelRequested = false;
        private IBackgroundTaskInstance _taskInstance;
        private BackgroundTaskDeferral _deferral;

        public override void Register()
        {
            var taskName = GetType().Name;
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                task.Value.Unregister(true);
            }

            if (!BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == taskName))
            {
                var builder = new BackgroundTaskBuilder()
                {
                    Name = taskName
                };

                builder.SetTrigger(CompletionGroup.Trigger);
                builder.Register();
            }
        }

        public override Task RunAsyncInternal(IBackgroundTaskInstance taskInstance)
        {
            if (taskInstance == null)
            {
                return null;
            }

            _deferral = taskInstance.GetDeferral();

            return Task.Run(() =>
            {
                if (taskInstance.TriggerDetails is BackgroundTransferCompletionGroupTriggerDetails details)
                {
                    int failedDownloads = 0;
                    int succeededDownloads = 0;
                    foreach (var download in details.Downloads)
                    {
                        if (download.IsFailed())
                        {
                            failedDownloads++;
                        }
                        else
                        {
                            succeededDownloads++;
                        }
                    }

                    ShowToastNotification(succeededDownloads, failedDownloads);
                }

                _taskInstance = taskInstance;
                ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(SampleTimerCallback), TimeSpan.FromSeconds(1));
            });
        }

        public override void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _cancelRequested = true;

            // TODO WTS: Insert code to handle the cancelation request here.
            // Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/handle-a-cancelled-background-task
        }

        private void SampleTimerCallback(ThreadPoolTimer timer)
        {
            if ((_cancelRequested == false) && (_taskInstance.Progress < 100))
            {
                _taskInstance.Progress += 10;
                Message = $"Background Task {_taskInstance.Task.Name} running";
            }
            else
            {
                timer.Cancel();

                if (_cancelRequested)
                {
                    Message = $"Background Task {_taskInstance.Task.Name} cancelled";
                }
                else
                {
                    Message = $"Background Task {_taskInstance.Task.Name} finished";
                }

                _deferral?.Complete();
            }
        }

        private void ShowToastNotification(int succeededDownloads, int failedDownloads)
        {
            var content = new ToastContent()
            {
                Launch = "ToastContentActivationParams",
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Downloading from background task"
                            },

                            new AdaptiveText()
                            {
                                 Text = $"Succeeded downloads: {succeededDownloads}, Failed downloads: {failedDownloads}"
                            }
                        }
                    }
                }
            };

            var toast = new ToastNotification(content.GetXml())
            {
                Tag = "ToastTag"
            };

            Singleton<ToastNotificationsService>.Instance.ShowToastNotification(toast);
        }
    }
}
