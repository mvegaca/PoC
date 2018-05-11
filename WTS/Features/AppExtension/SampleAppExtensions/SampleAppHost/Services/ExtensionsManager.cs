using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppExtensions;
using Windows.UI.Core;

namespace SampleAppHost.Services
{
    public class ExtensionsManager
    {
        private string _extensionContractName;
        private CoreDispatcher _dispatcher;
        private AppExtensionCatalog _catalog;

        public ObservableCollection<Extension> Extensions { get; } = new ObservableCollection<Extension>();

        public ExtensionsManager(string extensionContractName)
        {
            _extensionContractName = extensionContractName;
            _catalog = AppExtensionCatalog.Open(_extensionContractName);
            _dispatcher = null;
        }

        public async Task InitializeAsync(CoreDispatcher dispatcher)
        {
            if (_dispatcher != null)
            {
                throw new Exception($"Extension Manager for {_extensionContractName} is already initialized.");
            }
            _dispatcher = dispatcher;
            _catalog.PackageInstalled += OnPackageInstalled;
            _catalog.PackageUpdated += OnPackageUpdated;
            _catalog.PackageUninstalling += OnPackageUninstalling;
            _catalog.PackageUpdating += OnPackageUpdating;
            _catalog.PackageStatusChanged += OnPackageStatusChanged;

            await FindAndLoadExtensionsAsync();
        }

        private async Task FindAndLoadExtensionsAsync()
        {
            if (_dispatcher == null)
            {
                throw new Exception($"ExtensionManager for {_extensionContractName} is not initialized.");
            }

            IReadOnlyList<AppExtension> appExtensions = await _catalog.FindAllAsync();
            foreach (AppExtension extension in appExtensions)
            {
                await LoadExtensionAsync(extension);
            }
        }

        private async void OnPackageInstalled(AppExtensionCatalog sender, AppExtensionPackageInstalledEventArgs args)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                foreach (var extension in args.Extensions)
                {
                    await LoadExtensionAsync(extension);
                }
            });
        }

        private async void OnPackageUpdated(AppExtensionCatalog sender, AppExtensionPackageUpdatedEventArgs args)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                foreach (var extension in args.Extensions)
                {
                    await LoadExtensionAsync(extension);
                }
            });
        }

        private async void OnPackageUninstalling(AppExtensionCatalog sender, AppExtensionPackageUninstallingEventArgs args)
        {
            await RemoveExtensionsAsync(args.Package);
        }

        private async void OnPackageUpdating(AppExtensionCatalog sender, AppExtensionPackageUpdatingEventArgs args)
        {
            await UnloadExtensionsAsync(args.Package);
        }

        private async void OnPackageStatusChanged(AppExtensionCatalog sender, AppExtensionPackageStatusChangedEventArgs args)
        {
            if (!args.Package.Status.VerifyIsOK())
            {
                if (args.Package.Status.PackageOffline)
                {
                    await UnloadExtensionsAsync(args.Package);
                }
                else if (args.Package.Status.Servicing || args.Package.Status.DeploymentInProgress)
                {
                }
                else
                {
                    await RemoveExtensionsAsync(args.Package);
                }
            }
            else
            {
                await LoadExtensionsAsync(args.Package);
            }
        }

        private async Task LoadExtensionsAsync(Package package)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Extensions.Where(extension => extension.AppExtension.Package.Id.FamilyName == package.Id.FamilyName)
                          .ToList()
                          .ForEach(e => e.MarkAsLoaded());
            });
        }

        private async Task UnloadExtensionsAsync(Package package)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Extensions.Where(extension => extension.AppExtension.Package.Id.FamilyName == package.Id.FamilyName)
                          .ToList()
                          .ForEach(e => e.Unload());
            });
        }

        private async Task RemoveExtensionsAsync(Package package)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Extensions.Where(ext => ext.AppExtension.Package.Id.FamilyName == package.Id.FamilyName)
                          .ToList()
                          .ForEach(e =>
                          {
                              e.Unload();
                              Extensions.Remove(e);
                          });
            });
        }

        private async Task LoadExtensionAsync(AppExtension extension)
        {
            if (!extension.Package.Status.VerifyIsOK())
            {
                return;
            }

            var identifier = $"{extension.AppInfo.AppUserModelId}!{extension.Id}";
            var existingExtension = Extensions.Where(e => e.UniqueId == identifier).FirstOrDefault();
            if (existingExtension == null)
            {
                var newExtension = await Extension.FromAppExtensionAsync(extension);
                Extensions.Add(newExtension);
            }
            else
            {
                existingExtension.Unload();
                await existingExtension.UpdateAsync(extension);
            }
        }
    }
}
