using System;

using NavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavigationPaneProject.Views
{
    public sealed partial class GridPage : Page
    {
        public GridViewModel ViewModel { get; } = new GridViewModel();

        // TODO WTS: Change the grid as appropriate to your app.
        // For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        // You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted
        public GridPage()
        {
            InitializeComponent();
        }
    }
}
