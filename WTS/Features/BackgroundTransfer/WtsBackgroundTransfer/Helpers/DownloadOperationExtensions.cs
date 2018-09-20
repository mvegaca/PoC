using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;

namespace WtsBackgroundTransfer.Helpers
{
    public static class DownloadOperationExtensions
    {
        public static bool IsFailed(this DownloadOperation download)
        {
            var status = download.Progress.Status;
            if (status == BackgroundTransferStatus.Error || status == BackgroundTransferStatus.Canceled)
            {
                return true;
            }

            ResponseInformation response = download.GetResponseInformation();
            if (response.StatusCode != 200)
            {
                return true;
            }

            return false;
        }
    }
}
