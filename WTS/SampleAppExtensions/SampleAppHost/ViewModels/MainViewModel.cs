using System;
using System.Windows.Input;
using SampleAppHost.Helpers;
using SampleAppHost.Services;

namespace SampleAppHost.ViewModels
{
    public class MainViewModel : Observable
    {
        public ICommand DebugCommand { get; }

        public MainViewModel()
        {
            DebugCommand = new RelayCommand(OnDebugCommandExecute);
        }

        public async void OnDebugCommandExecute()
        {
            foreach (var extension in App.ExtensionsService.Extensions)
            {
                extension.Enable();
                var request = new ExtensionRequest();
                await request.AddParameterAsync("Parameter01", "Hello World");
                await request.AddParameterAsync("Parameter02", 2018);
                await request.AddParameterAsync("Parameter03", new DateTime(2018, 3, 19));

                var response = await extension.InvokeAsync(request);
                var response01 = await response.GetValueAsync<string>("Response01");
                var response02 = await response.GetValueAsync<int>("Response02");
                var response03 = await response.GetValueAsync<DateTime>("Response03");
            }
        }
    }
}
