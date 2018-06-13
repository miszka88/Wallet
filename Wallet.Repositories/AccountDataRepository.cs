using System;
using System.Net.Http;
using System.Threading.Tasks;
using Wallet.Domain.Repositories;

namespace Wallet.Repositories
{
    public class AccountDataRepository : IAccountDataRepository
    {
        public HttpClient _httpClient { get; set; }

        public AccountDataRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> GetUserAccounts(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode) return null;

            return response;
        }
    }
}
