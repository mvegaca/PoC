﻿using System.Collections.ObjectModel;
using System.Windows.Input;

using ImageGalleryApp.Helpers;
using ImageGalleryApp.Models;
using ImageGalleryApp.Services;
using ImageGalleryApp.Views;
using Windows.UI.Xaml.Controls;

namespace ImageGalleryApp.ViewModels
{
    public class ImageGalleryViewModel : Observable
    {
        public const string ImageGallerySelectedIdKey = "ImageGallerySelectedIdKey";

        private ObservableCollection<SampleImage> _source;
        private ICommand _itemSelectedCommand;

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => Set(ref _source, value);
        }

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand<ItemClickEventArgs>(OnsItemSelected));

        public ImageGalleryViewModel()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData();
        }

        private void OnsItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            NavigationService.Navigate<ImageGalleryDetailPage>(args.ClickedItem);
        }
    }
}
