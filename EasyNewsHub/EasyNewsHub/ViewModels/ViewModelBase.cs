using EasyNewsHub.Helpers;

namespace EasyNewsHub.ViewModels
{
    public abstract class ViewModelBase : Observable
    {
        private bool _isBusy;
        public bool IsBusy { get => _isBusy; set => Set(ref _isBusy, value); }
    }
}
