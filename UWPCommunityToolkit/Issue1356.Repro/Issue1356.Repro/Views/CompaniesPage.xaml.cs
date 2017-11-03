using System;
using System.Linq;

using Issue1356.Repro.ViewModels;

using Microsoft.Toolkit.Uwp.UI.Controls;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Issue1356.Repro.Views
{
    public sealed partial class CompaniesPage : Page
    {
        public CompaniesPage()
        {
            InitializeComponent();
        }

        private CompaniesViewModel ViewModel
        {
            get { return DataContext as CompaniesViewModel; }
        }

        private void MasterDetailsViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                ViewModel.ActiveItem = ViewModel.Items.First();
            }
        }
    }
}
