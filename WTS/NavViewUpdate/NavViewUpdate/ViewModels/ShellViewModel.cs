using System;
using System.Windows.Input;

using NavViewUpdate.Helpers;
using NavViewUpdate.Services;
using NavViewUpdate.Views;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace NavViewUpdate.ViewModels
{
    public class ShellViewModel : Observable
    {
        private NavigationView _navigationView;
        private object _selected;
        private ICommand _itemInvokedCommand;
        private ICommand _selectionChangedCommand;

        public object Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<NavigationViewItemInvokedEventArgs>(ItemInvoked));

        public ICommand SelectionChangedCommand => _selectionChangedCommand ?? (_selectionChangedCommand = new RelayCommand<NavigationViewSelectionChangedEventArgs>(OnSelectionChanged));

        public ShellViewModel()
        {
        }

        public void Initialize(Frame frame, NavigationView navigationView)
        {
            _navigationView = navigationView;
            NavigationService.Frame = frame;
            NavigationService.Navigated += Frame_Navigated;
        }

        private void OnSelectionChanged(NavigationViewSelectionChangedEventArgs args)
        {
            Navigate(args.IsSettingsSelected, args.SelectedItem);
        }

        private void ItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            Navigate(args.IsSettingsInvoked, args.InvokedItem);
        }

        private void Navigate(bool navigateToSettings, object newItem)
        {
            if (navigateToSettings)
            {
                NavigationService.Navigate(typeof(SettingsPage));
            }
            else
            {
                foreach (var menuItem in _navigationView.MenuItems)
                {
                    if(((NavigationViewItem)menuItem).Content.ToString() == newItem.ToString())
                    {
                        NavigationService.Navigate(Type.GetType(((NavigationViewItem)menuItem).Tag.ToString()));
                    }
                }
            }
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = _navigationView.SettingsItem;
            }
            else
            {
                foreach (var menuItem in _navigationView.MenuItems)
                {
                    if (((NavigationViewItem)menuItem).Tag.ToString() == e.SourcePageType.ToString())
                    {
                        Selected = menuItem;
                    }
                }
            }
        }
    }
}
