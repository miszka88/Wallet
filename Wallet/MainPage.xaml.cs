using Autofac;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Wallet.Authorization;
using Wallet.Common;
using Wallet.Common.Helpers;
using Wallet.Domain.Models;
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
        private readonly HttpClient _httpClient;
        private readonly IAuthorization _authorization;
        private readonly IAccountDataService _accountDataService;
        private readonly ICategoryService _categoryService;

        private ILocalStorage _localStorage;

        private ObservableCollection<GroupedUserAccounts> UserAccounts { get; } = new ObservableCollection<GroupedUserAccounts>();
        private ObservableCollection<MoneyTransactionClass> AccountTransactions { get; } = new ObservableCollection<MoneyTransactionClass>();

        public MainPage()
        {
            this.InitializeComponent();

            _httpClient = App.Container.Resolve<HttpClient>();
            _authorization = App.Container.Resolve<IAuthorization>();
            _accountDataService = App.Container.Resolve<IAccountDataService>();
            _categoryService = App.Container.Resolve<ICategoryService>();

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

            IEnumerable<UserAccountClass> userAccountsList = new List<UserAccountClass>();

            Task.Run(async () =>
            {
                //this.AccountsList.ItemsSource = accountsData;
                //var accountsList = await _accountDataService.GetUserAccountsList();
                //var walletsList = await _accountDataService.GetDefaultUserWallet();
                //await _accountDataService.AddTransaction(new MoneyTransaction(), 1);

                //await _categoryService.GetAll();

                //Debug.WriteLine(string.Join("\n", accountsData.Where(a => !string.IsNullOrWhiteSpace(a.UserAccount.DisplayName)).Select(x => $"Id:{x.UserAccount.Id} | DispayName:{x.UserAccount.DisplayName}")));

                //var accountId = accountsData
                //    .SingleOrDefault(a => a.UserAccount.DisplayName.Contains("gotówka")).UserAccount.Id;

                //await _accountDataService.GetAccountTransactionsById(accountId);

                var groupedAccounts = from account in await _accountDataService.GetUserAccountsData()
                                      group account by account.UserAccount.BankName into g
                                      select (new { GroupName = g.Key, Items = g });
                foreach (var g in groupedAccounts)
                {
                    GroupedUserAccounts userAccount = new GroupedUserAccounts();
                    userAccount.Key = g.GroupName;
                    userAccount.AddRange(g.Items);
                    UserAccounts.Add(userAccount);
                }
            }).Wait();
#endif
            UserAccountsList.Source = UserAccounts;
        }

        private async void AccountsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedUserAccount = (UserAccountClass)e.ClickedItem;

            if (selectedUserAccount != null) { AccountTransactions.Clear(); }
            else { return; }

            var transactions = await _accountDataService.GetAccountTransactionsById(selectedUserAccount.UserAccount.Id);

            foreach (var item in transactions)
            {
                AccountTransactions.Add(item);
            }
        }
    }
}
