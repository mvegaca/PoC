using System;
using System.Linq;
using System.Windows.Input;
using WinUI = Microsoft.UI.Xaml.Controls;

using NavigationViewWinUI.Helpers;
using NavigationViewWinUI.Services;
using NavigationViewWinUI.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.System;
using System.Collections.Generic;

namespace NavigationViewWinUI.ViewModels
{
    public class ShellViewModel : Observable
    {
        private bool _isBackEnabled;
        private WinUI.NavigationView _navigationView;
        private WinUI.NavigationViewItem _selected;
        private ICommand _itemInvokedCommand;

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }

        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked));

        public readonly KeyboardAccelerator AltLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);

        public readonly KeyboardAccelerator BackKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        public ShellViewModel()
        {
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            _navigationView = navigationView;
            NavigationService.Frame = frame;
            NavigationService.Navigated += Frame_Navigated;
            _navigationView.BackRequested += OnBackRequested;
            keyboardAccelerators.Add(AltLeftKeyboardAccelerator);
            keyboardAccelerators.Add(BackKeyboardAccelerator);
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            var item = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            NavigationService.Navigate(pageType);
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private void Frame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            Selected = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            ToolTipService.SetToolTip(keyboardAccelerator, string.Empty);
            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }
    }
}
