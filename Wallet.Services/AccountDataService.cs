using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Common;
using Wallet.Common.Helpers;
using Wallet.Domain.Models;
using Wallet.Domain.Repositories;
using Wallet.Domain.Services;


namespace Wallet.Services
{
    public class AccountDataService : IAccountDataService
    {
        private readonly IAccountDataRepository _accountDataRepository;
        private readonly ILocalStorage _localStorage;

        private KeyValuePair<string, string> ApiKeyParam { get; }

        public AccountDataService(IAccountDataRepository accountDataRepository, ILocalStorage localStorage)
        {
            _accountDataRepository = accountDataRepository;
            _localStorage = localStorage;

            ApiKeyParam = new KeyValuePair<string, string>("api_key", _localStorage.ReadVariableValue("ApiKey").ToString());
        }
        public async Task<IEnumerable<UserAccountClass>> GetUserAccountsData()
        {
            var uri = UriBuilderHelper.BuildUri(AccountAction.UserAccounts, ResponseType.Json, ApiKeyParam);
            var result = await _accountDataRepository.Get(uri);

            if (result == null) return null;

            var rawData = await result.Content.ReadAsStringAsync();

            var accountsData = JsonConvert.DeserializeObject<UserAccountClass[]>(rawData);
#if DEBUG
            foreach (var accout in accountsData)
            {
                Debug.WriteLine($"{accout.UserAccount.BankName}|{accout.UserAccount.DisplayName}|{accout.UserAccount.Balance} {accout.UserAccount.CurrencyName}|{accout.UserAccount.IsDefaultWallet}");
            }
#endif

            return accountsData;
        }

        public async Task<IEnumerable<MoneyTransactionClass>> GetAccountTransactionsById(long accountId)
        {
            var accountIdParam = new KeyValuePair<string, string>("user_account_id", accountId.ToString());
            var uri = UriBuilderHelper.BuildUri(AccountAction.Transactions, ResponseType.Json, ApiKeyParam, accountIdParam);
            var result = await _accountDataRepository.Get(uri);

            if (result == null) return null;

            var rawData = await result.Content.ReadAsStringAsync();

            var accountTransactions = JsonConvert.DeserializeObject<MoneyTransactionClass[]>(rawData);

#if DEBUG
            foreach (var t in accountTransactions)
            {
                Debug.WriteLine($"Transaction.Id:{t.MoneyTransaction.Id}|{t.MoneyTransaction.Description}|{t.MoneyTransaction.Amount} {t.MoneyTransaction.CurrencyName}");
            }
#endif

            return accountTransactions;
        }

        public async Task AddTransaction(MoneyTransaction transaction, long walletId)
        {
#if DEBUG
            var userDefaultWalletId = await GetDefaultUserWallet();
            var userAccountId = userDefaultWalletId.Single().Key;

            transaction.Direction = TransactionDirection.Type.Deposit;
            if (transaction.Direction == TransactionDirection.Type.Deposit) transaction.CategoryId = 7748005;
            else transaction.CategoryId = 6328643;

            var transactionData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("money_transaction[category_id]", transaction.CategoryId.ToString()), // category id for deposit: 7748005, for withdrawal: 6328643
                new KeyValuePair<string, string>("money_transaction[currency_amount]", "90"),
                new KeyValuePair<string, string>("money_transaction[direction]", transaction.Direction.ToString()),
                new KeyValuePair<string, string>("money_transaction[name]", "fake_data"),
                new KeyValuePair<string, string>("money_transaction[tag_string]", "APP_WALLET"),
                new KeyValuePair<string, string>("money_transaction[transaction_on]", new DateTime(2018,06,17).ToString()),
                new KeyValuePair<string, string>("money_transaction[user_account_id]", userAccountId.ToString()),
                new KeyValuePair<string, string>("money_transaction[client_assigned_id]", DateTime.Now.Ticks.ToString())
            };

#else
            var transactionData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("money_transaction[user_account_id]", transaction.UserAccountId.ToString()),
                new KeyValuePair<string, string>("money_transaction[category_id]", transaction.CategoryId.ToString()),
                new KeyValuePair<string, string>("money_transaction[currency_amount]", transaction.CurrencyAmount),
                new KeyValuePair<string, string>("money_transaction[currency_name]", transaction.CurrencyName),
                new KeyValuePair<string, string>("money_transaction[direction]", transaction.Direction),
                new KeyValuePair<string, string>("money_transaction[tag_string]", transaction.TagString), // can be a List<string>
                new KeyValuePair<string, string>("money_transaction[name]", transaction.Name),
                new KeyValuePair<string, string>("money_transaction[transaction_on]", transaction.TransactionOn), // dd-MM-yyyy
                new KeyValuePair<string, string>("money_transaction[client_assigned_id]", DateTime.Now.Ticks.ToString())

            };

#endif
            var uri = UriBuilderHelper.BuildUri(AccountAction.Transactions, ResponseType.Json, ApiKeyParam);

            await _accountDataRepository.AddTransaction(transactionData, uri);
        }

        public async Task<IDictionary<long, string>> GetUserAccountsList()
        {
            var accountsData = await GetUserAccountsData();

            var accountsList = accountsData.ToDictionary(x => x.UserAccount.Id, x => x.UserAccount.DisplayName);

#if DEBUG
            Debug.WriteLine("Accounts List:");
            foreach (var item in accountsList)
            {
                Debug.WriteLine($"{item.Key} | {item.Value}");
            }
#endif

            return accountsList;
        }

        public async Task<IDictionary<long, string>> GetDefaultUserWallet()
        {
            var accountsData = await GetUserAccountsData();

            var accountsList = accountsData
                .Where(ad => ad.UserAccount.IsDefaultWallet)
                .ToDictionary(x => x.UserAccount.Id, x => x.UserAccount.DisplayName);

#if DEBUG
            Debug.WriteLine("Wallets List:");
            foreach (var item in accountsList)
            {
                Debug.WriteLine($"{item.Key} | {item.Value}");
            }
#endif

            return accountsList;
        }
    }
}
