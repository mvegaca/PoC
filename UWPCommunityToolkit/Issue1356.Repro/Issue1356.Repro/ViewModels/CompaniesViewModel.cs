using System;
using System.Linq;
using System.Threading.Tasks;

using Caliburn.Micro;

using Issue1356.Repro.Helpers;
using Issue1356.Repro.Services;

namespace Issue1356.Repro.ViewModels
{
    public class CompaniesViewModel : Conductor<CompaniesDetailViewModel>.Collection.OneActive
    {
        protected override async void OnInitialize()
        {
            base.OnInitialize();

            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            Items.Clear();

            var data = await SampleDataService.GetSampleModelDataAsync();

            Items.AddRange(data.Select(d => new CompaniesDetailViewModel(d)));
        }
    }
}
