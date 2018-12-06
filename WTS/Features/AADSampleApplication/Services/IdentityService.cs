using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace AADSampleApplication.Services
{
    public static class IdentityService
    {
        // TODO: Configure your Client ID
        // 1. Register your app https://apps.dev.microsoft.com/portal/register-app
        // 2. Configure UWP Platform
        // 3. Define App scopes.
        private const string _clientId = "f73f7920-86dc-4f37-9771-3d0c57a01716";

        private static string[] _scopes = new string[] { "user.read" };

        private static readonly PublicClientApplication _client = new PublicClientApplication(_clientId);

        public static async Task<AuthenticationResult> LogInAsync()
        {
            AuthenticationResult authResult = null;

            var accounts = await _client.GetAccountsAsync();

            try
            {
                authResult = await _client.AcquireTokenSilentAsync(_scopes, accounts.FirstOrDefault());
            }
            catch (MsalUiRequiredException)
            {
                try
                {
                    authResult = await _client.AcquireTokenAsync(_scopes);
                }
                catch (MsalException)
                {
                }
            }
            catch (Exception)
            {
            }

            return authResult;
        }

        public static async Task LogOutAsync()
        {
            var accounts = await _client.GetAccountsAsync();
            IAccount firstAccount = accounts.FirstOrDefault();
            try
            {
                await _client.RemoveAsync(firstAccount);
            }
            catch (MsalException)
            {
            }
        }
    }
}
