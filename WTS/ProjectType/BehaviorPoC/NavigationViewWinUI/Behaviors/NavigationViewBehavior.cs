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
                var header = page.GetValue(HeaderProperty);
                if (header != null)
                {
                    AssociatedObject.Header = header;
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
