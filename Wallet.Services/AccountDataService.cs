﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly ICategoryService _categoryService;
        private readonly ILocalStorage _localStorage;

        private KeyValuePair<string, string> ApiKeyParam { get; }

        public AccountDataService(IAccountDataRepository accountDataRepository, ILocalStorage localStorage, ICategoryService categoryService)
        {
            _accountDataRepository = accountDataRepository;
            _categoryService = categoryService;
            _localStorage = localStorage;

            ApiKeyParam = new KeyValuePair<string, string>("api_key", _localStorage.ReadVariableValue("ApiKey")?.ToString());
        }
        public async Task<IEnumerable<UserAccountObject>> GetUserAccountsData()
        {
            var uri = UriBuilderHelper.BuildUri(AccountAction.UserAccounts, ResponseType.Json, ApiKeyParam);
            var result = await _accountDataRepository.GetAll(uri);

            if (result == null) return null;

            var rawData = await result.Content.ReadAsStringAsync();

            var accountsData = JsonConvert.DeserializeObject<UserAccountObject[]>(rawData);

            return accountsData;
        }

        public async Task<ObservableCollection<MoneyTransactionObject>> GetTransactionsByAccountId(long accountId)
        {
            var accountIdParam = new KeyValuePair<string, string>("user_account_id", accountId.ToString());
            var uri = UriBuilderHelper.BuildUri(AccountAction.Transactions, ResponseType.Json, ApiKeyParam, accountIdParam);
            var result = await _accountDataRepository.GetAll(uri);

            if (result == null) return null;

            var rawData = await result.Content.ReadAsStringAsync();

            var accountTransactions = JsonConvert.DeserializeObject<MoneyTransactionObject[]>(rawData);

            return new ObservableCollection<MoneyTransactionObject>(accountTransactions);
        }

        public async Task AddTransaction(MoneyTransaction transaction, long walletId)
        {
#if DEBUG
            var userDefaultWalletId = await GetDefaultWalletId();

            transaction.Direction = TransactionDirection.Type.Withdrawal;
            var categories = await _categoryService.GetAll();
            // categoryId for deposit: 7748005, for withdrawal: 6328643
            transaction.CategoryId = 6328643;
            transaction.CategoryName = categories.Single(c => c.Key == transaction.CategoryId).Value;

            var transactionData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("money_transaction[user_account_id]", userDefaultWalletId.ToString()),
                new KeyValuePair<string, string>("money_transaction[category_id]", transaction.CategoryId.ToString()),
                new KeyValuePair<string, string>("money_transaction[currency_amount]", "90"),
                //new KeyValuePair<string, string>("money_transaction[direction]", transaction.Direction.ToString().ToLower()),
                new KeyValuePair<string, string>("money_transaction[name]", "test-transaction-data"),
                new KeyValuePair<string, string>("money_transaction[tag_string]", "APP_WALLET"),
                new KeyValuePair<string, string>("money_transaction[transaction_on]", DateTime.Now.ToString()),
                new KeyValuePair<string, string>("money_transaction[client_assigned_id]", DateTime.Now.Ticks.ToString())
            };

#else
            var userDefaultWalletId = await GetDefaultWalletId();

            if (!IsCategoryValidForTransactionDirection(await _categoryService.GetByTransactionDirection(transaction.Direction), transaction.CategoryId))
                throw new ArgumentException($"Category: {transaction.CategoryName} does not belong to {transaction.Direction} group.");

            var transactionData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("money_transaction[user_account_id]", walletId.ToString()),
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

            return accountsList;
        }

        public async Task<IDictionary<long, string>> GetDefaultUserWallet()
        {
            var accountsData = await GetUserAccountsData();

            var accountsList = accountsData
                .Where(ad => ad.UserAccount.IsDefaultWallet)
                .ToDictionary(x => x.UserAccount.Id, x => x.UserAccount.DisplayName);

            return accountsList;
        }

        public async Task<long> GetDefaultWalletId()
        {
            var accounts = await GetUserAccountsData();
#if DEBUG
            return accounts.Single(x => x.UserAccount.DisplayName == "test_wallet").UserAccount.Id;
#else
            return accounts.Single(x => x.UserAccount.IsDefaultWallet).UserAccount.Id;
#endif
        }

        private bool IsCategoryValidForTransactionDirection(IDictionary<long, string> categories, long categoryId)
        {
            var isValid = categories.Keys.Contains(categoryId);

            return isValid;
        }

        public async Task<ObservableCollection<GroupedUserAccount>> GetGroupedUserAccounts()
        {
            var userAccoutnsData = await GetUserAccountsData();
            var groupedUserAccoutnsData = userAccoutnsData.GroupBy(item => item.UserAccount.BankName)
                         .Select(group => new GroupedUserAccount(group) { Key = group.Key });

            return new ObservableCollection<GroupedUserAccount>(groupedUserAccoutnsData);
        }

        public async Task<ObservableCollection<GroupedMoneyTransaction>> GetGroupedTransactionsByAccountId(long accountId)
        {
            var transactionsData = await GetTransactionsByAccountId(accountId);

            var groupedMoneyTransactions = transactionsData.GroupBy(item => item.MoneyTransaction.TransactionOn)
                .Select(group => new GroupedMoneyTransaction(group) { Key = group.Key });

            return new ObservableCollection<GroupedMoneyTransaction>(groupedMoneyTransactions);
        }

        public async Task RemoveTransaction(long transactionId)
        {
            var transactionToRemoveParams = new KeyValuePair<string, string>(string.Empty, transactionId.ToString());

            var uri = UriBuilderHelper.BuildUri(AccountAction.Transactions, ResponseType.Json, ApiKeyParam, transactionToRemoveParams);

            var isSuccessRemoved = await _accountDataRepository.RemoveTransaction(transactionId, uri);

            if (isSuccessRemoved)
            {
                await GetTransactionsByAccountId(await GetDefaultWalletId());
            }
        }

    }
}
