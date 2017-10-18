using AppStudio.DataProviders.Rss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNewsHub.Models
{
    public class FeedModel
    {
        public string Id { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public IEnumerable<RssSchema> Items { get; set; }

        public void Update(FeedModel feed)
        {
            Id = feed.Id;
            Name = feed.Name;
            Description = feed.Description;
            Url = feed.Url;
            Items = feed.Items;
        }
    }
}
