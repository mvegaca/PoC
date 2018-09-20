using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace WtsBackgroundTransfer.Helpers
{
    public static class FileHelper
    {
        public static async Task<StorageFile> GetFromPicturesLibraryAsync(string fileName)
        {
            return await KnownFolders.PicturesLibrary.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
        }
    }
}
