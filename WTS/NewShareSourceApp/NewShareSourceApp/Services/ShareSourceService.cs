using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;

namespace NewShareSourceApp.Services
{
    public static class ShareSourceService
    {
        // TODO WTS: See more documentation about how to share data from your app
        // https://docs.microsoft.com/windows/uwp/app-to-app/share-data
        private static Action<DataRequestedEventArgs> _fillShareContentAction;
        private static DataTransferManager dataTransferManager;
        private static bool _isInitialized;

        public static void ShareData(ShareSourceConfig data)
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            _fillShareContentAction = FillShareContent(data);
            DataTransferManager.ShowShareUI();
        }

        private static void Initialize()
        {
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
            _isInitialized = true;
        }

        private static Action<DataRequestedEventArgs> FillShareContent(ShareSourceConfig data)
        {
            return (args) =>
            {
                var requestData = args.Request.Data;
                if (!string.IsNullOrEmpty(data.Title))
                {
                    requestData.Properties.Title = data.Title;
                }
                if (!string.IsNullOrEmpty(data.Description))
                {
                    requestData.Properties.Description = data.Description;
                }
                var storageItems = new List<IStorageItem>();
                foreach (var dataItem in data.Items)
                {
                    switch (dataItem.DataType)
                    {
                        case ShareSourceItemType.Text:
                            requestData.SetText(dataItem.Text);
                            break;
                        case ShareSourceItemType.WebLink:
                            requestData.SetWebLink(dataItem.WebLink);
                            break;
                        case ShareSourceItemType.ApplicationLink:
                            requestData.SetApplicationLink(dataItem.ApplicationLink);
                            break;
                        case ShareSourceItemType.Html:
                            var htmlFormat = HtmlFormatHelper.CreateHtmlFormat(dataItem.Html);
                            requestData.SetHtmlFormat(htmlFormat);
                            break;
                        case ShareSourceItemType.Image:
                            FillImage(requestData, dataItem.Image, storageItems);
                            break;
                        case ShareSourceItemType.StorageItems:
                            FillStorageItems(requestData, dataItem.StorageItems, storageItems);
                            break;
                        case ShareSourceItemType.DeferredContent:
                            FillDeferredContent(requestData, dataItem.DeferredDataFormatId, dataItem.GetDeferredDataAsyncFunc);
                            break;
                    }
                }
                if (storageItems.Any())
                {
                    requestData.SetStorageItems(storageItems);
                }
            };
        }

        private static void FillImage(DataPackage requestData, StorageFile image, List<IStorageItem> storageItems)
        {
            storageItems.Add(image);
            var streamReference = RandomAccessStreamReference.CreateFromFile(image);
            requestData.Properties.Thumbnail = streamReference;
            requestData.SetBitmap(streamReference);
        }

        private static void FillStorageItems(DataPackage requestData, IEnumerable<IStorageItem> sourceItems, List<IStorageItem> storageItems)
        {
            foreach (var item in sourceItems)
            {
                storageItems.Add(item);
            }
        }

        private static void FillDeferredContent(DataPackage requestData, string deferredDataFormatId, Func<Task<object>> getDeferredDataAsyncFunc)
        {
            requestData.SetDataProvider(deferredDataFormatId, async (providerRequest) =>
            {
                var deferral = providerRequest.GetDeferral();
                try
                {
                    var deferredData = await getDeferredDataAsyncFunc();
                    providerRequest.SetData(deferredData);
                }
                finally
                {
                    deferral.Complete();
                }
            });
        }

        private static void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // This event will be fired when the share operation starts.
            // We need to add data to DataRequestedEventArgs through the _fillShareContentAction
            args.Request.Data.ShareCompleted += OnShareCompleted;
            var deferral = args.Request.GetDeferral();
            try
            {
                _fillShareContentAction?.Invoke(args);
            }
            finally
            {
                deferral.Complete();
            }
        }

        private static void OnShareCompleted(DataPackage sender, ShareCompletedEventArgs args)
        {
            // This event will be fired when Share Operation will finish
            // TODO WTS: If you need to handle any action when de data is shared implement on this method
        }
    }
}
