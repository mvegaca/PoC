using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AppExtensionHost.Helpers;
using AppExtensionHost.Services;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace AppExtensionHost.ViewModels
{
    public class ExtensionsViewModel : Observable
    {
        private string _parameter01;
        private int _parameter02;
        private string _parameter03;
        private DateTime _parameter03DateTime = DateTime.Now;
        private ICommand _sendRequestCommand;

        public string Parameter01
        {
            get => _parameter01;
            set => Set(ref _parameter01, value);
        }

        public int Parameter02
        {
            get => _parameter02;
            set => Set(ref _parameter02, value);
        }

        public string Parameter03
        {
            get => _parameter03;
            set
            {
                Set(ref _parameter03, value);
                var dateValues = value.Split('/');
                Parameter03DateTime = new DateTime(int.Parse(dateValues[2]), int.Parse(dateValues[1]), int.Parse(dateValues[0]));
            }
        }

        public DateTime Parameter03DateTime
        {
            get => _parameter03DateTime;
            private set => Set(ref _parameter03DateTime, value);
        }

        public ICommand SendRequestCommand => _sendRequestCommand ?? (_sendRequestCommand = new RelayCommand(OnSendRequest));

        public readonly ObservableCollection<ExtensionViewModel> Extensions = new ObservableCollection<ExtensionViewModel>();

        public ExtensionsViewModel()
        {
            Parameter01 = "Hello World";
            Parameter02 = 2018;
            Parameter03 = DateTime.Now.ToString("dd/MM/yyyy");
            InitializeExtensions();
        }

        public void InitializeExtensions()
        {
            Extensions.Clear();
            foreach (var ext in App.ExtensionsService.Extensions)
            {
                if (ext.IsEnabled)
                {
                    Extensions.Add(new ExtensionViewModel(ext));
                }
            }
        }

        private async void OnSendRequest()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async () =>
            {
                var request = await GetRequestAsync();
                foreach (var extension in Extensions)
                {
                    await extension.InvokeAsync(request);
                }
            });
        }

        private async Task<ExtensionRequest> GetRequestAsync()
        {
            var request = new ExtensionRequest();
            await request.AddParameterAsync("Parameter01", Parameter01);
            await request.AddParameterAsync("Parameter02", Parameter02);
            await request.AddParameterAsync("Parameter03", Parameter03DateTime);
            return request;
        }
    }
}
