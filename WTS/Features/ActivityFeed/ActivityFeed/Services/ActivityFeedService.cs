using System;
using System.Threading.Tasks;
using ActivityFeed.Activation;
using Windows.ApplicationModel.UserActivities;
using Windows.UI;
using Windows.UI.Shell;

namespace ActivityFeed.Services
{
    public static class ActivityFeedService
    {
        private static UserActivitySession _currentUserActivitySession;

        public static Task AddUserActivityAsync(string activityId, SchemeActivationData activationData, string displayText, string description = null, Color? backgroundColor = null)
        {
            return AddUserActivityAsync(activityId, activationData, displayText, description, backgroundColor, null);
        }

        public static Task AddUserActivityAsync(string activityId, SchemeActivationData activationData, string displayText, IAdaptiveCard adaptiveCard)
        {
            return AddUserActivityAsync(activityId, activationData, displayText, null, null, adaptiveCard);
        }

        //AdaptativeCards on Windows
        //https://docs.microsoft.com/en-us/adaptive-cards/get-started/windows
        // Your application should name activities in such a way that same ID is generated each time the user is in a particular location in the app. For example, if your application is page-based, use an identifier for the page, if it’s document based, use the name of the doc (or a hash of the name).
        private static async Task AddUserActivityAsync(string activityId, SchemeActivationData activationData, string displayText, string description = null, Color? backgroundColor = null, IAdaptiveCard adaptiveCard = null)
        {
            if (string.IsNullOrEmpty(activityId))
            {
                throw new ArgumentNullException(nameof(activityId));
            }
            else if (activationData == null)
            {
                throw new ArgumentNullException(nameof(activationData));
            }
            else if (string.IsNullOrEmpty(displayText))
            {
                throw new ArgumentNullException(nameof(displayText));
            }

            //Get the default UserActivityChannel and query it for our UserActivity. If the activity doesn't exist, one is created.
            var channel = UserActivityChannel.GetDefault();
            var activationUri = activationData.BuildUri();
            var activity = await channel.GetOrCreateUserActivityAsync(activityId);
            activity.ActivationUri = activationUri;

            //Populate minimum required properties
            activity.VisualElements.DisplayText = displayText;
            if (!backgroundColor.HasValue)
            {
                backgroundColor = default(Color);
            }
            if (!string.IsNullOrEmpty(description))
            {
                activity.VisualElements.Description = description;
            }
            activity.VisualElements.BackgroundColor = backgroundColor.Value;
            activity.VisualElements.Content = adaptiveCard;

            await activity.SaveAsync();

            //Dispose of any current UserActivitySession, and create a new one.
            _currentUserActivitySession?.Dispose();
            _currentUserActivitySession = activity.CreateSession();
        }
    }
}
