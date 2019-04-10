using System;

using IntegratedSuspendAndResumeMockup.Helpers;

namespace IntegratedSuspendAndResumeMockup.ViewModels
{
    public class MainViewModel : Observable
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        public MainViewModel()
        {
        }
    }
}
