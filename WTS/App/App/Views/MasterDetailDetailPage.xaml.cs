using System;

using App.Models;
using App.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace App.Views
{
    public sealed partial class MasterDetailDetailPage : Page
    {
        public MasterDetailDetailViewModel ViewModel { get; } = new MasterDetailDetailViewModel();

        public MasterDetailDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Item = e.Parameter as SampleOrder;
        }
    }
}
