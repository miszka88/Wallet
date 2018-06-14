using System;
using System.Diagnostics;
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
        public async Task<HttpResponseMessage> Get(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode) return null;
#if DEBUG
            Debug.WriteLine($"Reposotory.Get | Server response: {(int)response.StatusCode} {response.StatusCode}");
#endif

            return response;
        }
    }
}
