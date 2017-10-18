using System;

using EasyNewsHub.Helpers;
using System.Threading.Tasks;
using Windows.Storage;
using System.Collections.ObjectModel;
using System.Linq;
using EasyNewsHub.Services;
using System.Windows.Input;
using System.Collections;
using System.Collections.Generic;
using EasyNewsHub.Models;
using Windows.UI.Xaml.Controls;

namespace EasyNewsHub.ViewModels
{
    public class NewsFeedsViewModel : ViewModelBase
    {
        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick));

        public ObservableCollection<FeedViewModel> Feeds { get; } = new ObservableCollection<FeedViewModel>();

        public NewsFeedsViewModel()
        {
        }

        public async Task LoadAsync()
        {
            var cacheData = await ApplicationData.Current.LocalFolder.ReadAsync<IEnumerable<FeedModel>>(Constants.SettingsKey_NewsFeeds);
            if (cacheData != null)
            {
                cacheData.ToList().ForEach(f => Feeds.Add(new FeedViewModel(f)));
            }
            else
            {
                SampleDataService.SampleFeeds.ToList().ForEach(f => Feeds.Add(f));
                await SaveAsync();
            }
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            var feed = args.ClickedItem as FeedViewModel;
            if (feed != null)
            {
                NavigationService.Navigate<Views.NewsListPage>(feed.Id);
            }
        }

        private async Task SaveAsync()
        {
            IEnumerable<FeedModel> feedModels = Feeds.Select(f => f.GetModel());
            await ApplicationData.Current.LocalFolder.SaveAsync(Constants.SettingsKey_NewsFeeds, feedModels);
        }
    }
}
