using System;

using NavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavigationPaneProject.Views
{
    public sealed partial class ChartPage : Page
    {
        public ChartViewModel ViewModel { get; } = new ChartViewModel();

        // TODO WTS: Change the chart as appropriate to your app.
        // For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        public ChartPage()
        {
            InitializeComponent();
        }
    }
}
