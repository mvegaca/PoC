using System;
using AppStudio.DataProviders.Rss;

namespace EasyNewsHub.ViewModels
{
    public class NewItemViewModel
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string ExtraImageUrl { get; set; }
        public string MediaUrl { get; set; }
        public string FeedUrl { get; set; }
        public string Author { get; set; }
        public DateTime PublishDate { get; set; }

        public NewItemViewModel(RssSchema model)
        {
            Title = model.Title;
            Summary = model.Summary;
            Content = model.Content;
            ImageUrl = model.ImageUrl;
            ExtraImageUrl = model.ExtraImageUrl;
            MediaUrl = model.MediaUrl;
            FeedUrl = model.FeedUrl;
            Author = model.Author;
            PublishDate = model.PublishDate;
        }

        public RssSchema GetModel()
        {
            return new RssSchema()
            {
                Title = Title,
                Summary = Summary,
                Content = Content,
                ImageUrl = ImageUrl,
                ExtraImageUrl = ExtraImageUrl,
                MediaUrl = MediaUrl,
                FeedUrl = FeedUrl,
                Author = Author,
                PublishDate = PublishDate
            };
        }
    }
}
