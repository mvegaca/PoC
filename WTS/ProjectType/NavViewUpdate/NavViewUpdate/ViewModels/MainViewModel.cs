using System;

using NavViewUpdate.Helpers;

namespace NavViewUpdate.ViewModels
{
    public class MainViewModel : Observable
    {
        public string Title { get; set; }

        public MainViewModel()
        {
            Title = "Main";
        }
    }
}
