using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserActivities;
using Windows.UI;
using Windows.UI.Shell;
using ActivityFeed.Activation;

namespace ActivityFeed.Services
{
    // For more info about UserActivities in Timeline see
    // https://blogs.windows.com/buildingapps/2017/12/19/application-engagement-windows-timeline-user-activities/#q4oyyjCE45qW8MMl.97
    // For more info about UserActivities with AdaptativeCards see
    // https://docs.microsoft.com/adaptive-cards/get-started/windows
    public static class ActivityFeedService
    {
        private static UserActivitySession _currentUserActivitySession;

        public static async Task AddUserActivityAsync(string activityId, SchemeActivationData activationData, string displayText, string description = null, Color? backgroundColor = null)
        {
            var activity = await CreateUserActivityAsync(activityId, activationData, displayText);
            if (!string.IsNullOrEmpty(description))
            {
                activity.VisualElements.Description = description;
            }
            if (!backgroundColor.HasValue)
            {
                backgroundColor = default(Color);
            }
            activity.VisualElements.BackgroundColor = backgroundColor.Value;
            await SaveUserActivityAsync(activity);
        }

        public static async Task AddUserActivityAsync(string activityId, SchemeActivationData activationData, string displayText, IAdaptiveCard adaptiveCard)
        {
            var activity = await CreateUserActivityAsync(activityId, activationData, displayText);
            activity.VisualElements.Content = adaptiveCard;
            await SaveUserActivityAsync(activity);
        }

        private static async Task<UserActivity> CreateUserActivityAsync(string activityId, SchemeActivationData activationData, string displayText)
        {
            if (string.IsNullOrEmpty(activityId)) throw new ArgumentNullException(nameof(activityId));            
            if (activationData == null) throw new ArgumentNullException(nameof(activationData));
            if (string.IsNullOrEmpty(displayText)) throw new ArgumentNullException(nameof(displayText));            

            var channel = UserActivityChannel.GetDefault();
            var activationUri = activationData.BuildUri();
            var activity = await channel.GetOrCreateUserActivityAsync(activityId);
            activity.ActivationUri = activationUri;

            activity.VisualElements.DisplayText = displayText;
            return activity;
        }

        private static async Task SaveUserActivityAsync(UserActivity activity)
        {
            await activity.SaveAsync();

            //Dispose of any current UserActivitySession, and create a new one.
            _currentUserActivitySession?.Dispose();
            _currentUserActivitySession = activity.CreateSession();
        }
    }
}
