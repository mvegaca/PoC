using AppStudio.DataProviders;
using AppStudio.DataProviders.Rss;
using EasyNewsHub.Helpers;
using EasyNewsHub.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNewsHub.ViewModels
{
    public class FeedViewModel : Observable
    {
        private RssDataProvider _rssDataProvider;

        private string _id;
        public string Id{ get => _id; set => Set(ref _id, value); }

        private string _name;
        public string Name { get => _name; set => Set(ref _name, value); }

        private string _url;
        public string Url { get => _url; set => Set(ref _url, value); }

        private string _description;
        public string Description { get => _description; set => Set(ref _description, value); }

        public ObservableCollection<NewItemViewModel> Items { get; } = new ObservableCollection<NewItemViewModel>();

        public FeedViewModel(FeedModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Description = model.Description;
            Url = model.Url;
            if (model.Items != null && model.Items.Any())
            {
                model.Items.ToList().ForEach(m => Items.Add(new NewItemViewModel(m)));
            }
        }

        public async Task<bool> LoadAsync()
        {

            if (!Items.Any())
            {
                await GetItemsAsync();
                return true;
            }
            return false;

        }

        public FeedModel GetModel()
        {
            return new FeedModel()
            {
                Id = Id,
                Name = Name,                
                Description = Description,
                Url = Url,
                Items = Items.Select(i => i.GetModel())
            };
        }

        private async Task GetItemsAsync()
        {
            string rssQuery = "http://www.blogger.com/feeds/6781693/posts/default";
            int maxRecordsParam = 20;
            string orderBy = "PublishDate";

            _rssDataProvider = new RssDataProvider();

            var config = new RssDataConfig()
            {
                Url = new Uri(rssQuery, UriKind.Absolute),
                OrderBy = orderBy,
                OrderDirection = SortDirection.Descending
            };

            var items = await _rssDataProvider.LoadDataAsync(config, maxRecordsParam);
            foreach (var item in items)
            {
                Items.Add(new NewItemViewModel(item));
            }
        }
    }
}
