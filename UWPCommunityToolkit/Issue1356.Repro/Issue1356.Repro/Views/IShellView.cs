using System;

using Caliburn.Micro;

namespace Issue1356.Repro.Views
{
    public interface IShellView
    {
        INavigationService CreateNavigationService(WinRTContainer container);
    }
}
