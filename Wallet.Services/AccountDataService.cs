using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task<IEnumerable<UserAccountClass>> GetUserAccounts()
        {
            var uri = UriBuilderHelper.BuildUri(AccountAction.UserAccounts, ResponseType.Json, ApiKeyParam);
            var result = await _accountDataRepository.Get(uri);

            if (result == null) return null;

            var rawData = await result.Content.ReadAsStringAsync();

            var accountList = JsonConvert.DeserializeObject<UserAccountClass[]>(rawData);
#if DEBUG
            foreach (var accout in accountList)
            {
                Debug.WriteLine($"{accout.UserAccount.BankName}|{accout.UserAccount.DisplayName}|{accout.UserAccount.Balance} {accout.UserAccount.CurrencyName}|{accout.UserAccount.IsDefaultWallet}");
            }
#endif

            return accountList;
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
    }
}
