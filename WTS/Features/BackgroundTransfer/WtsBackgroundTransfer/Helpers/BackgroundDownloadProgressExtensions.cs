using Windows.Networking.BackgroundTransfer;

namespace WtsBackgroundTransfer.Helpers
{
    public static class BackgroundDownloadProgressExtensions
    {
        public static double GetPercent(this BackgroundDownloadProgress progress)
        {
            if (progress.TotalBytesToReceive > 0)
            {
                return progress.BytesReceived * 100d / progress.TotalBytesToReceive;
            }

            return 0;
        }
    }
}
