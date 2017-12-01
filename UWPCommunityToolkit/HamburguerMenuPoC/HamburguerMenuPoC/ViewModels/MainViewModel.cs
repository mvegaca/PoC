using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamburguerMenuPoC.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _selected;

        public string Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Selected"));
            }
        }

        private Boolean? _useNavigationViewWhenPossible = true;

        public Boolean? UseNavigationViewWhenPossible
        {
            get { return _useNavigationViewWhenPossible; }
            set
            {
                _useNavigationViewWhenPossible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UseNavigationViewWhenPossible"));
            }
        }

        public ObservableCollection<string> Items { get; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
        }

        public void LoadData()
        {
            for (int i = 1; i <= 10; i++)
            {
                Items.Add($"Item {i}");
            }
            Selected = Items.First();
        }
    }
}
