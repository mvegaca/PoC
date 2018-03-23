using System;

using Caliburn.Micro;

namespace CaliPoCApp.Views
{
    public interface IShellView
    {
        INavigationService CreateNavigationService(WinRTContainer container);
    }
}
