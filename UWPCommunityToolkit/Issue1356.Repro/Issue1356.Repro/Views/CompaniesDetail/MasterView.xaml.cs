using System;

using Issue1356.Repro.ViewModels;

namespace Issue1356.Repro.Views.CompaniesDetail
{
    public sealed partial class MasterView
    {
        public MasterView()
        {
            InitializeComponent();
        }

        public CompaniesDetailViewModel ViewModel => DataContext as CompaniesDetailViewModel;
    }
}
