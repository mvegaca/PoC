using System;
using System.Collections.ObjectModel;

using NavigationViewWinUI.Helpers;
using NavigationViewWinUI.Models;
using NavigationViewWinUI.Services;

namespace NavigationViewWinUI.ViewModels
{
    public class ChartViewModel : Observable
    {
        public ChartViewModel()
        {
        }

        public ObservableCollection<DataPoint> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetChartSampleData();
            }
        }
    }
}
