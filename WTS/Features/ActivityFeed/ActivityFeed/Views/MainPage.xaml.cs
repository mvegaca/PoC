using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ActivityFeed.Activation;
using ActivityFeed.Services;
using ActivityFeed.ViewModels;

namespace ActivityFeed.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // This code should not be part of generated code. This will be included in a ActivityFeed documentation.
            var activationData = new SchemeActivationData
            (
                pageType: typeof(MainPage),
                parameters: new Dictionary<string, string>()
                {
                    {"paramName1", "paramValue1"},
                    {"paramName2", "paramValue2"},
                    {"paramName3", "paramValue3"}
                }
            );

            //// Way 1
            //// Fill the UserActivity with Title (mandatory), Description (optional) and BackgroundColor (optional)
            //await ActivityFeedService.AddUserActivityAsync(
            //    activityId: nameof(MainPage),
            //    activationData: activationData,
            //    displayText: $"Work on {nameof(MainPage)}",
            //    description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna.",
            //    backgroundColor: Colors.DarkBlue);

            // Way 2
            // Create the UserActivity with the Title (mandatory) and a new AdaptiveCard (mandatory)
            var displayText = $"Work on {nameof(MainPage)}";
            var description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna.";
            var imageUrl = "http://adaptivecards.io/content/cats/2.png";
            var adaptiveCard = AdaptiveCardsService.CreateAdaptiveCardSample(displayText, description, imageUrl);

            await ActivityFeedService.AddUserActivityAsync(
                activityId: nameof(MainPage),
                activationData: activationData,
                displayText: displayText,
                adaptiveCard: adaptiveCard,
                backgroundColor: Colors.DarkRed);
        }
    }
}
