using System;
using System.Threading.Tasks;
using AppExtensionHost.Helpers;
using AppExtensionHost.Services;

namespace AppExtensionHost.ViewModels
{
    public class ExtensionViewModel : Observable
    {
        private string _response01;
        private int _response02;
        private string _response03;
        private readonly Extension _extension;

        public string Response01
        {
            get => _response01;
            set => Set(ref _response01, value);
        }

        public int Response02
        {
            get => _response02;
            set => Set(ref _response02, value);
        }

        public string Response03
        {
            get => _response03;
            set => Set(ref _response03, value);
        }

        public ExtensionViewModel(Extension extension)
        {
            _extension = extension;
        }

        public async Task InvokeAsync(ExtensionRequest request)
        {
            // Reasons to null response:
            // 1. - The extension is not loaded.
            // 2. - The AppServiceConnectionStatus is not Success
            // 3. - The AppServiceResponseStatus is not Success
            // 5. - Calling the App Service failed.
            var response = await _extension.InvokeAsync(request);
            if (response != null)
            {
                Response01 = await response.GetValueAsync<string>("Response01");
                Response02 = await response.GetValueAsync<int>("Response02");
                var response03DateTime = await response.GetValueAsync<DateTime>("Response03");
                Response03 = response03DateTime.ToString("dd/MM/yyyy");
            }
        }
    }
}
