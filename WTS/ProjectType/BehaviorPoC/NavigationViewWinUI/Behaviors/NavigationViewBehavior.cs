using System;
using System.Linq;
using WinUI = Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using NavigationViewWinUI.Helpers;
using NavigationViewWinUI.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;

namespace NavigationViewWinUI.Behaviors
{
    public class NavigationViewBehavior : Behavior<WinUI.NavigationView>
    {
        private static NavigationViewBehavior _current;

        public DataTemplate DefaultHeaderTemplate { get; set; }

        public object DefaultHeader
        {
            get { return GetValue(DefaultHeaderProperty); }
            set { SetValue(DefaultHeaderProperty, value); }
        }

        public static readonly DependencyProperty DefaultHeaderProperty = DependencyProperty.Register("DefaultHeader", typeof(object), typeof(NavigationViewBehavior), new PropertyMetadata(null, (d, e) => _current.UpdateHeader()));



        public static bool GetShowHeader(Page item)
        {
            return (bool)item.GetValue(ShowHeaderProperty);
        }

        public static void SetShowHeader(Page item, bool value)
        {
            item.SetValue(ShowHeaderProperty, value);
        }

        public static readonly DependencyProperty ShowHeaderProperty =
            DependencyProperty.RegisterAttached("ShowHeader", typeof(bool), typeof(NavigationViewBehavior), new PropertyMetadata(true, (d, e) => _current.UpdateHeader()));

        public static object GetHeader(Page item)
        {
            return item.GetValue(HeaderProperty);
        }

        public static void SetHeader(Page item, object value)
        {
            item.SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header", typeof(object), typeof(NavigationViewBehavior), new PropertyMetadata(null, (d, e) => _current.UpdateHeader()));

        public static DataTemplate GetHeaderTemplate(Page item)
        {
            return (DataTemplate)item.GetValue(HeaderTemplateProperty);
        }

        public static void SetHeaderTemplate(Page item, DataTemplate value)
        {
            item.SetValue(HeaderTemplateProperty, value);
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(NavigationViewBehavior), new PropertyMetadata(null, (d, e) => _current.UpdateHeaderTemplate()));

        protected override void OnAttached()
        {
            base.OnAttached();
            _current = this;
            NavigationService.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            UpdateHeader();
            UpdateHeaderTemplate();
        }

        private void UpdateHeader()
        {
            if (NavigationService.Frame.Content is Page page)
            {
                if (GetShowHeader(page))
                {
                    var headerFromPage = page.GetValue(HeaderProperty);
                    if (headerFromPage != null)
                    {
                        AssociatedObject.Header = headerFromPage;
                    }
                    else
                    {
                        AssociatedObject.Header = DefaultHeader;
                    }
                    AssociatedObject.AlwaysShowHeader = true;
                }
                else
                {
                    AssociatedObject.Header = null;
                    AssociatedObject.AlwaysShowHeader = false;
                }
            }
        }

        private void UpdateHeaderTemplate()
        {
            if (NavigationService.Frame.Content is Page page)
            {
                var headerTemplate = page.GetValue(HeaderTemplateProperty) as DataTemplate;
                AssociatedObject.HeaderTemplate = headerTemplate ?? DefaultHeaderTemplate;
            }
        }
    }
}
