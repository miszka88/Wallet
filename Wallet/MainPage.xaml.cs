using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Wallet.Authorization;
using Wallet.Common;
using Wallet.Common.Helpers;
using Wallet.Domain.Services;
using Wallet.Repositories;
using Wallet.Services;
using Wallet.Storage;
using Windows.UI.Xaml.Controls;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace Wallet
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthorization _authorization;
        private readonly ILocalStorage _localStorage;

        public MainPage()
        {
            _httpClient = new HttpClient();
            _localStorage = new LocalStorage();

            if (string.IsNullOrWhiteSpace(_localStorage.ReadVariableValue("ApiKey").ToString()))
            {
                Debug.WriteLine("ApiKey not stored.");

                var apiKey = _localStorage.ReadVariableValue("ApiKey");

                _authorization = new Auth(new AuthorizationService(new AuthorizationRepository(_httpClient), new LocalStorage()));

                var loginParams = new List<KeyValuePair<string, string>>
                {
                };

                var uri = UriBuilderHelper.BuildUri(AccountAction.Session, ResponseType.Json);


                Task.Run(() => _authorization.Authorize(_httpClient, loginParams, uri)).Wait();
            }
            Debug.WriteLine("ApiKey read from local storage.");

            this.InitializeComponent();
        }
    }
}
