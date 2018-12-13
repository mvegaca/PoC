﻿using System;

using MVVMLightNetStandard.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MVVMLightNetStandard.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
