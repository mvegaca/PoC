using System;

using Issue1356.Repro.ViewModels;

namespace Issue1356.Repro.Views.CompaniesDetail
{
    public sealed partial class DetailsView
    {
        public DetailsView()
        {
            InitializeComponent();
        }

        public CompaniesDetailViewModel ViewModel => DataContext as CompaniesDetailViewModel;
    }
}
