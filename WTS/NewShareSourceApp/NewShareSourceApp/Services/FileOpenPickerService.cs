using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace NewShareSourceApp.Services
{
    public static class FileOpenPickerService
    {
        public static async Task<StorageFile> PickImage()
        {
            FileOpenPicker imagePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter = { ".jpg", ".png", ".bmp", ".gif", ".tif" }
            };
            return await imagePicker.PickSingleFileAsync();
        }

        public static async Task<IReadOnlyList<StorageFile>> PickMultipleImages()
        {
            FileOpenPicker filesPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter = { ".jpg", ".png", ".bmp", ".gif", ".tif" }
            };
            return await filesPicker.PickMultipleFilesAsync();
        }
    }
}
