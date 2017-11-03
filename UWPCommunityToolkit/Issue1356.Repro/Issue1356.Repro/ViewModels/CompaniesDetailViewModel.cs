using System;

using Caliburn.Micro;

using Issue1356.Repro.Models;

namespace Issue1356.Repro.ViewModels
{
    public class CompaniesDetailViewModel : Screen
    {
        public CompaniesDetailViewModel(SampleOrder item)
        {
            Item = item;
        }

        public SampleOrder Item { get; }
    }
}
