using System;
using AdaptiveCards;
using Windows.UI.Shell;

namespace ActivityFeed.Services
{
    public static class AdaptiveCardsService
    {
        // TODO WTS: Change this to configure your own adaptive card
        // For more info about adaptive cards see http://adaptivecards.io/
        public static IAdaptiveCard CreateAdaptiveCardSample(string displayText, string description, string imageUrl = null)
        {
            var adaptiveCard = new AdaptiveCard();
            var columns = new AdaptiveColumnSet();
            var firstColumn = new AdaptiveColumn() { Width = "auto" };
            var secondColumn = new AdaptiveColumn() { Width = "*" };
            if (!string.IsNullOrEmpty(imageUrl))
            {
                firstColumn.Items.Add(new AdaptiveImage()
                {
                    Url = new Uri(imageUrl),
                    Size = AdaptiveImageSize.Medium
                });
            }
            secondColumn.Items.Add(new AdaptiveTextBlock()
            {
                Text = displayText,
                Weight = AdaptiveTextWeight.Bolder,
                Size = AdaptiveTextSize.Large
            });
            secondColumn.Items.Add(new AdaptiveTextBlock()
            {
                Text = description,
                Size = AdaptiveTextSize.Medium,
                Weight = AdaptiveTextWeight.Lighter,
                Wrap = true
            });
            columns.Columns.Add(firstColumn);
            columns.Columns.Add(secondColumn);
            adaptiveCard.Body.Add(columns);
            return AdaptiveCardBuilder.CreateAdaptiveCardFromJson(adaptiveCard.ToJson());
        }
    }
}
