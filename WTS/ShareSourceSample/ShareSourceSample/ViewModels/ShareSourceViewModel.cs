using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ShareSourceSample.Helpers;

namespace ShareSourceSample.ViewModels
{
    public class ShareSourceViewModel : Observable
    {
        private ICommand _shareCommand;
        public ICommand ShareCommand => _shareCommand ?? (_shareCommand = new RelayCommand(OnShare));

        private string _selectedOption;
        public string SelectedOption
        {
            get { return _selectedOption; }
            set { Set(ref _selectedOption, value); }
        }


        public IEnumerable<string> ShareOptions
        {
            get
            {
                yield return "ShareSource_ShareTypeText".GetLocalized();
                yield return "ShareSource_ShareTypeLink".GetLocalized();
            }
        }

        public ShareSourceViewModel()
        {
            SelectedOption = ShareOptions.First();
        }

        private void OnShare()
        {

        }
    }
}
