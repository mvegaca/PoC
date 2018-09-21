using System;
using Windows.Networking.BackgroundTransfer;
using WtsBackgroundTransfer.Helpers;

namespace WtsBackgroundTransfer.Models
{
    public class DownloadInfo : Observable
    {
        private string _fileName;
        private string _status;
        private double _percent;

        public string FileName
        {
            get { return _fileName; }
            set { Set(ref _fileName, value); }
        }

        public string Status
        {
            get { return _status; }
            set { Set(ref _status, value); }
        }

        public double Percent
        {
            get { return _percent; }
            set { Set(ref _percent, value); }
        }

        public DownloadInfo(DownloadOperation download)
        {
            FileName = download.ResultFile.Name;
            Update(download);
        }

        public void Update(DownloadOperation download)
        {
            Status = GetStatusString(download);
            Percent = download.Progress.GetPercent();
        }

        private string GetStatusString(DownloadOperation download)
        {
            return download.Progress.Status.ToString();
        }
    }
}
