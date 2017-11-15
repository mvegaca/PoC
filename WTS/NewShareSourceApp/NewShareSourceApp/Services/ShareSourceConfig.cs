﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace NewShareSourceApp.Services
{
    public class ShareSourceConfig
    {
        public string Title { get; set; }

        public string Description { get; set; }

        internal List<ShareSourceItem> Items { get; }

        public ShareSourceConfig()
        {
            Items = new List<ShareSourceItem>();
        }

        public void SetText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"The parameter {nameof(text)} is null or empty");
            }
            Items.Add(ShareSourceItem.FromText(text));
        }

        public void SetWebLink(Uri webLink)
        {
            if (webLink == null)
            {
                throw new ArgumentNullException(nameof(webLink));
            }
            Items.Add(ShareSourceItem.FromWebLink(webLink));
        }

        // TODO WTS: If you want to share a link to your application be sure
        // to do the following to configure activation by uri
        //
        // Protocol must be added in Package.appxmanifest in Declarations/protocol
        // i.e.
        // <uap:Protocol Name="my-app-name">
        //      <uap:Logo>Assets\smallTile-sdk.png</uap:Logo>
        //      <uap:DisplayName>MyApp</uap:DisplayName>
        // </uap:Protocol>
        //
        // applicationLink must belong to the registered protocol
        // new Uri("my-app-name:navigate?page=MainPage")
        public void SetApplicationLink(Uri applicationLink)
        {
            if (applicationLink == null)
            {
                throw new ArgumentNullException("Application Link to share is null.");
            }
            Items.Add(ShareSourceItem.FromApplicationLink(applicationLink));
        }

        public void SetHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentNullException("Html to share is empty.");

            }
            Items.Add(ShareSourceItem.FromHtml(html));
        }

        public void SetImage(StorageFile image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("Image to share is null.");
            }
            Items.Add(ShareSourceItem.FromImage(image));
        }

        public void SetStorageItems(IEnumerable<IStorageItem> storageItems)
        {
            if (storageItems == null || !storageItems.Any())
            {
                throw new ArgumentNullException("There are not storage items to share.");
            }
            Items.Add(ShareSourceItem.FromStorageItems(storageItems));
        }

        // TODO WTS: Use this method add content to share when you do not want to process the data until the target app actually requests it.
        // formatId must be a const value from StandardDataFormats class.
        // getDeferredDataAsyncFunc is the function that returns the object you want to share.
        public void SetDeferredContent(string deferredDataFormatId, Func<Task<object>> getDeferredDataAsyncFunc)
        {
            if (string.IsNullOrEmpty(deferredDataFormatId))
            {
                throw new ArgumentNullException("Deferred data Id to share is empty");
            }
            Items.Add(ShareSourceItem.FromDeferredContent(deferredDataFormatId, getDeferredDataAsyncFunc));
        }
    }
}
