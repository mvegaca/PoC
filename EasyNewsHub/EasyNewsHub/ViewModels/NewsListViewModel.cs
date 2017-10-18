using System;
using System.Threading.Tasks;
using EasyNewsHub.Helpers;
using Windows.Storage;
using System.Linq;
using System.Collections.Generic;
using EasyNewsHub.Models;

namespace EasyNewsHub.ViewModels
{
    public class NewsListViewModel : Observable
    {

        private FeedViewModel _feed;
        public FeedViewModel Feed
        {
            get => _feed;
            private set => Set(ref _feed, value);
        }

        public NewsListViewModel()
        {
        }

        public async Task LoadAsync(string feedId)
        {
            var cacheData = await ApplicationData.Current.LocalFolder.ReadAsync<IEnumerable<FeedModel>>(Constants.SettingsKey_NewsFeeds);
            var feedModel = cacheData.FirstOrDefault(f => f.Id == feedId);
            if (feedModel != null)
            {
                Feed = new FeedViewModel(feedModel);
                var updated = await Feed.LoadAsync();
                if (updated)
                {
                    feedModel.Update(Feed.GetModel());
                    await ApplicationData.Current.LocalFolder.SaveAsync(Constants.SettingsKey_NewsFeeds, cacheData);
                }                
            }
        }
    }
}
