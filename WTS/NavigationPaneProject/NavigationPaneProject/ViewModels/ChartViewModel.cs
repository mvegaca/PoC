using System;
using System.Collections.ObjectModel;

using NavigationPaneProject.Helpers;
using NavigationPaneProject.Models;
using NavigationPaneProject.Services;

namespace NavigationPaneProject.ViewModels
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
