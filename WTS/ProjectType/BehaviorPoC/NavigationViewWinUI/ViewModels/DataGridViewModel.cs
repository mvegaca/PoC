using System;
using System.Collections.ObjectModel;

using NavigationViewWinUI.Helpers;
using NavigationViewWinUI.Models;
using NavigationViewWinUI.Services;

namespace NavigationViewWinUI.ViewModels
{
    public class DataGridViewModel : Observable
    {
        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetGridSampleData();
            }
        }
    }
}
