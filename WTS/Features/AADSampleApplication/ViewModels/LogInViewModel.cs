using AADSampleApplication.Helpers;
using AADSampleApplication.Services;

namespace AADSampleApplication.ViewModels
{
    public class LogInViewModel : Observable
    {
        private bool _isBussy;
        private bool _isUserLoggedIn;
        private string _userAccount;
        private RelayCommand _logInCommand;
        private RelayCommand _logOutCommand;

        public bool IsBussy
        {
            get { return _isBussy; }
            set
            {
                Set(ref _isBussy, value);
                LogInCommand.OnCanExecuteChanged();
                LogOutCommand.OnCanExecuteChanged();
            }
        }

        public bool IsUserLoggedIn
        {
            get { return _isUserLoggedIn; }
            set
            {
                Set(ref _isUserLoggedIn, value);
                LogInCommand.OnCanExecuteChanged();
                LogOutCommand.OnCanExecuteChanged();
            }
        }

        public string UserAccount
        {
            get { return _userAccount; }
            set { Set(ref _userAccount, value); }
        }

        public RelayCommand LogInCommand => _logInCommand ?? (_logInCommand = new RelayCommand(LogIn, CanLogIn));

        private bool CanLogIn() => !IsBussy && !IsUserLoggedIn;

        public RelayCommand LogOutCommand => _logOutCommand ?? (_logOutCommand = new RelayCommand(LogOut, CanLogOut));

        private bool CanLogOut() => !IsBussy && IsUserLoggedIn;

        public LogInViewModel()
        {
        }

        private async void LogIn()
        {
            IsBussy = true;
            var result = await IdentityService.LogInAsync();
            if (result != null)
            {
                IsUserLoggedIn = true;
                UserAccount = result.Account.Username;
            }
            IsBussy = false;
        }

        private async void LogOut()
        {
            IsBussy = true;
            await IdentityService.LogOutAsync();
            UserAccount = string.Empty;
            IsUserLoggedIn = false;
            IsBussy = false;
        }
    }
}
