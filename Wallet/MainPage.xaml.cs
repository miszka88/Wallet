using Autofac;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Wallet.Authorization;
using Wallet.Common;
using Wallet.Common.Helpers;
using Wallet.Domain.Services;
using Windows.UI.Xaml.Controls;


//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace Wallet
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HttpClient _httpClient;
        private IAuthorization _authorization;
        private ILocalStorage _localStorage;

        public MainPage()
        {
            this.InitializeComponent();

            _httpClient = App.Container.Resolve<HttpClient>();
            _authorization = App.Container.Resolve<IAuthorization>();
            _localStorage = App.Container.Resolve<ILocalStorage>();

            if (string.IsNullOrWhiteSpace(_localStorage.ReadVariableValue("ApiKey").ToString()))
            {
                Debug.WriteLine("ApiKey not stored.");

                var apiKey = _localStorage.ReadVariableValue("ApiKey");

                var loginParams = new List<KeyValuePair<string, string>>
                {
                };

                var uri = UriBuilderHelper.BuildUri(AccountAction.Session, ResponseType.Json);


                Task.Run(() => _authorization.Authorize(_httpClient, loginParams, uri)).Wait();
            }
            else
            {
                Debug.WriteLine("ApiKey read from local storage.");
                Debug.WriteLine($"ApiKey Value: {_localStorage.ReadVariableValue("ApiKey").ToString()}");
            }
        }
    }
}
