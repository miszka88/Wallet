﻿using Autofac;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private ObservableCollection<GroupedUserAccount> GroupedUserAccounts { get; set; } = new ObservableCollection<GroupedUserAccount>();
        private ObservableCollection<GroupedMoneyTransaction> GroupedAccountTransactions { get; set; } = new ObservableCollection<GroupedMoneyTransaction>();

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



#endif
            Task.Run(async () =>
            {
                GroupedUserAccounts = await _accountDataService.GetGroupedUserAccounts();
            }).Wait();

            UserAccountsList.Source = GroupedUserAccounts;
        }

        private async void AccountsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedUserAccount = (UserAccountObject)e.ClickedItem;

            if (selectedUserAccount != null) { GroupedAccountTransactions.Clear(); }
            else { return; }

            GroupedAccountTransactions = await _accountDataService.GetGroupedTransactionsByAccountId(selectedUserAccount.UserAccount.Id);

            AccountTransactionsList.Source = GroupedAccountTransactions;
        }
    }
}
