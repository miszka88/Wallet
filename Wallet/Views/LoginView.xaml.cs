using Autofac;
using System.Collections.Generic;
using System.Net.Http;
using Wallet.Authorization;
using Wallet.Common;
using Wallet.Common.Helpers;
using Wallet.Domain.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wallet.Views
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthorization _authorization;
        private ILocalStorage _localStorage;

        private object ApiKey { get; }

        public LoginView()
        {
            InitializeComponent();

            _httpClient = App.Container.Resolve<HttpClient>();
            _authorization = App.Container.Resolve<IAuthorization>();
            _localStorage = App.Container.Resolve<ILocalStorage>();
#if DEBUG
            _localStorage.RemoveStoredValue("ApiKey");
#else
            ApiKey = _localStorage.ReadVariableValue("ApiKey");
#endif
        }

        private void Grid_Loading(FrameworkElement sender, object args)
        {
            if (ApiKey != null)
            {
                Frame.Navigate(typeof(MainPage), ApiKey);
            }
        }

        private async void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            var loginParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("email", TxtUserName.Text),
                    new KeyValuePair<string, string>("password", TxtPassword.Password)
                };

            var uri = UriBuilderHelper.BuildUri(AccountAction.Session, ResponseType.Json);

            await _authorization.Authorize(_httpClient, loginParams, uri);

            if (_localStorage.ReadVariableValue("ApiKey") != null)
                Frame.Navigate(typeof(MainPage));
        }

        private void TxtPassword_PasswordChanging(PasswordBox sender, PasswordBoxPasswordChangingEventArgs args)
        {
            BtnLogIn.IsEnabled = IsFormValid();
        }

        private void TxtUserName_MyTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            BtnLogIn.IsEnabled = IsFormValid();
        }

        private bool IsFormValid() => TxtUserName.IsValid && TxtPassword.Password.Length > 0;
    }
}
