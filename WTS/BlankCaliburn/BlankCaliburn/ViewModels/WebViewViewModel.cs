﻿using System;
using System.Windows.Input;

using BlankCaliburn.Helpers;

using Caliburn.Micro;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BlankCaliburn.ViewModels
{
    public class WebViewViewModel : Screen
    {
        // TODO WTS: Set the URI of the page to show by default
        private const string DefaultUrl = "https://developer.microsoft.com/en-us/windows/apps";

        private Uri _source;

        public Uri Source
        {
            get { return _source; }
            set { Set(ref _source, value); }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                if (value)
                {
                    IsShowingFailedMessage = false;
                }

                Set(ref _isLoading, value);
                IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _isLoadingVisibility;

        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            set { Set(ref _isLoadingVisibility, value); }
        }

        private bool _isShowingFailedMessage;

        public bool IsShowingFailedMessage
        {
            get
            {
                return _isShowingFailedMessage;
            }

            set
            {
                if (value)
                {
                    IsLoading = false;
                }

                Set(ref _isShowingFailedMessage, value);
                FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private Visibility _failedMesageVisibility;

        public Visibility FailedMesageVisibility
        {
            get { return _failedMesageVisibility; }
            set { Set(ref _failedMesageVisibility, value); }
        }

        public void NavCompleted(WebViewNavigationCompletedEventArgs e)
        {
            IsLoading = false;

            NotifyOfPropertyChange(() => CanGoBack);
            NotifyOfPropertyChange(() => CanGoForward);
        }

        public void NavFailed(WebViewNavigationFailedEventArgs e)
        {
            // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = true;

            NotifyOfPropertyChange(() => CanGoBack);
            NotifyOfPropertyChange(() => CanGoForward);
        }

        public void Retry()
        {
            IsShowingFailedMessage = false;
            IsLoading = true;

            _webView?.Refresh();
        }

        public void GoBack() => _webView?.GoBack();

        public bool CanGoBack => _webView == null ? false : _webView.CanGoBack;

        public void GoForward() => _webView?.GoForward();

        public bool CanGoForward => _webView?.CanGoForward ?? false;

        public void RefreshBrowser() => _webView?.Refresh();

        public async void OpenInBrowser() => await Windows.System.Launcher.LaunchUriAsync(Source);

        private WebView _webView;

        public WebViewViewModel()
        {
            IsLoading = true;
            Source = new Uri(DefaultUrl);
        }

        public void Initialize(WebView webView)
        {
            _webView = webView;
        }
    }
}
