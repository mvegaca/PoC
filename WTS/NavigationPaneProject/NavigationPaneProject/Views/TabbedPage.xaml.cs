using System;

using NavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavigationPaneProject.Views
{
    public sealed partial class TabbedPage : Page
    {
        public TabbedViewModel ViewModel { get; } = new TabbedViewModel();

        public TabbedPage()
        {
            InitializeComponent();
        }
    }
}
