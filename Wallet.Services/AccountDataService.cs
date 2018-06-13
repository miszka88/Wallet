using Newtonsoft.Json;
using System.Collections.Generic;
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

        private string ApiKey { get; }

        public AccountDataService(IAccountDataRepository accountDataRepository, ILocalStorage localStorage)
        {
            _accountDataRepository = accountDataRepository;
            _localStorage = localStorage;

            ApiKey = _localStorage.ReadVariableValue("ApiKey").ToString();
        }
        public async Task<IEnumerable<UserAccountClass>> GetUserAccounts()
        {
            var uri = UriBuilderHelper.BuildUri(AccountAction.UserAccounts, ResponseType.Json, new KeyValuePair<string, string>("api_key", ApiKey));
            var result = await _accountDataRepository.GetUserAccounts(uri);

            if (result == null) return null;

            var rawData = await result.Content.ReadAsStringAsync();

            var accountList = JsonConvert.DeserializeObject<UserAccountClass[]>(rawData);

            return accountList;
        }
    }
}
