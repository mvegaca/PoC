using System;

using ShareSourceSample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ShareSourceSample.Views
{
    public sealed partial class ShareSourcePage : Page
    {
        public ShareSourceViewModel ViewModel { get; } = new ShareSourceViewModel();

        public ShareSourcePage()
        {
            InitializeComponent();
        }
    }
}
