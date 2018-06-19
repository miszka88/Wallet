using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Wallet.Domain.Repositories;

namespace Wallet.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly HttpClient _httpClient;

        public CategoryRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Get(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri);

            Debug.WriteLine($"CategoryRepository | Get(Uri uri) | GET | {(int)response.StatusCode}-{response.StatusCode}");

            if (!response.IsSuccessStatusCode) return null;

            return response;
        }
    }
}
