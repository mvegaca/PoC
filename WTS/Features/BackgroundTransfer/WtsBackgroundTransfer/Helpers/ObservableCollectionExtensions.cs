using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace WtsBackgroundTransfer.Helpers
{
    public static class ObservableCollectionExtensions
    {
        public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate)
        {
            var itemsToRemove = collection.Where(predicate).ToList();
            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }
    }
}
