using Windows.Networking.BackgroundTransfer;

namespace WtsBackgroundTransfer.BackgroundTasks
{
    public interface IBackgroundTransferBackgroundTask
    {
        BackgroundTransferCompletionGroup GetCompletionGroup();
    }
}
