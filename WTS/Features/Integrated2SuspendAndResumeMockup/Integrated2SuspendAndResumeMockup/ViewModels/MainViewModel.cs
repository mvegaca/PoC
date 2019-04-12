﻿using System;

using Integrated2SuspendAndResumeMockup.Helpers;

namespace Integrated2SuspendAndResumeMockup.ViewModels
{
    public class MainViewModel : Observable
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        public MainViewModel()
        {
        }
    }
}
