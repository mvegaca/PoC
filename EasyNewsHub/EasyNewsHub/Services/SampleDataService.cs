using EasyNewsHub.Models;
using EasyNewsHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNewsHub.Services
{
    public static class SampleDataService
    {
        public static IEnumerable<FeedViewModel> SampleFeeds
        {
            get
            {
                yield return new FeedViewModel(new FeedModel()
                {
                    Id = "",
                    Name = "Default blog",
                    Description = "",
                    Url = "http://www.blogger.com/feeds/6781693/posts/default"
                });
            }
        }
    }
}
