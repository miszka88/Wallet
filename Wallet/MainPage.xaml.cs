using Autofac;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private IAccountDataService _accountDataService;

        private ILocalStorage _localStorage;

        public MainPage()
        {
            this.InitializeComponent();

            _httpClient = App.Container.Resolve<HttpClient>();
            _authorization = App.Container.Resolve<IAuthorization>();
            _accountDataService = App.Container.Resolve<IAccountDataService>();

            _localStorage = App.Container.Resolve<ILocalStorage>();


            var apiKey = _localStorage.ReadVariableValue("ApiKey");
#if DEBUG
            if (apiKey == null)
            {
                Debug.WriteLine("ApiKey not stored.");

                var loginParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("email", ""),
                    new KeyValuePair<string, string>("password", "")
                };

                var uri = UriBuilderHelper.BuildUri(AccountAction.Session, ResponseType.Json);


                Task.Run(() => _authorization.Authorize(_httpClient, loginParams, uri)).Wait();
            }
            else
            {
                Debug.WriteLine("ApiKey read from local storage.");
            }

            Task.Run(async () =>
            {
                var accountList = await _accountDataService.GetUserAccounts();

                Debug.WriteLine(string.Join("\n", accountList.Where(a => !string.IsNullOrWhiteSpace(a.UserAccount.DisplayName)).Select(x => $"Id:{x.UserAccount.Id} | DispayName:{x.UserAccount.DisplayName}")));

                var accountId = accountList
                    .SingleOrDefault(a => a.UserAccount.DisplayName.Contains("gotówka")).UserAccount.Id;

                await _accountDataService.GetAccountTransactionsById(accountId);
            }).Wait();
#endif
        }
    }
}
