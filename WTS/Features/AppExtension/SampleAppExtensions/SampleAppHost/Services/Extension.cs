using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleAppHost.Helpers;
using Windows.ApplicationModel.AppExtensions;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace SampleAppHost.Services
{
    public class Extension
    {
        private PropertySet _properties;
        private string _serviceName;
        private readonly object _sync = new object();

        public AppExtension AppExtension { get; private set; }

        public bool IsEnabled { get; private set; }

        public bool IsOffline { get; private set; }

        public bool IsLoaded { get; private set; }

        public BitmapImage Logo { get; private set; }

        public string UniqueId { get; private set; }

        private Extension(AppExtension appExtension)
        {
            AppExtension = appExtension;
        }

        public static async Task<Extension> FromAppExtensionAsync(AppExtension appExtension)
        {
            var extension = new Extension(appExtension);
            await extension.LoadExtensionDataAsync();
            return extension;
        }

        public async Task UpdateAsync(AppExtension extension)
        {
            if (UniqueId != $"{extension.AppInfo.AppUserModelId}!{extension.Id}")
            {
                return;
            }
            AppExtension = extension;
            await LoadExtensionDataAsync();
        }

        public void MarkAsLoaded()
        {
            if (!AppExtension.Package.Status.VerifyIsOK())
            {
                return;
            }
            IsEnabled = true;
            if (!IsLoaded)
            {
                IsLoaded = true;
                IsOffline = false;
            }
        }

        public void Enable()
        {
            IsEnabled = true;
            MarkAsLoaded();
        }

        public void Unload()
        {
            lock (_sync)
            {
                if (IsLoaded)
                {
                    if (!AppExtension.Package.Status.VerifyIsOK() && !AppExtension.Package.Status.PackageOffline)
                    {
                        IsOffline = true;
                    }

                    IsLoaded = false;
                }
            }
        }

        public void Disable()
        {
            if (IsEnabled)
            {
                IsEnabled = false;
                Unload();
            }
        }

        public async Task<StorageFolder> GetPublicFolderAsync()
        {
            return await AppExtension.GetPublicFolderAsync();
        }

        public async Task<ExtensionResponse> InvokeAsync(ExtensionRequest request)
        {
            if (IsLoaded)
            {
                try
                {
                    using (var connection = new AppServiceConnection())
                    {
                        connection.AppServiceName = _serviceName;
                        connection.PackageFamilyName = AppExtension.Package.Id.FamilyName;

                        var status = await connection.OpenAsync();
                        if (status == AppServiceConnectionStatus.Success)
                        {
                            var response = await connection.SendMessageAsync(request.GetValueSet());
                            if (response.Status == AppServiceResponseStatus.Success)
                            {
                                return new ExtensionResponse(response.Message);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Calling the App Service failed
                    return null;
                }
            }
            return null;
        }

        private async Task LoadExtensionDataAsync()
        {
            UniqueId = $"{AppExtension.AppInfo.AppUserModelId}!{AppExtension.Id}";

            var fileStream = await (AppExtension.AppInfo.DisplayInfo.GetLogo(new Size(1, 1))).OpenReadAsync();
            BitmapImage logo = new BitmapImage();
            logo.SetSource(fileStream);
            Logo = logo;

            _properties = await AppExtension.GetExtensionPropertiesAsync() as PropertySet;

            _serviceName = null;
            if (_properties != null && _properties.ContainsKey("Service"))
            {
                PropertySet serviceProperty = _properties["Service"] as PropertySet;
                _serviceName = serviceProperty["#text"].ToString();
            }
            MarkAsLoaded();
        }
    }
}
