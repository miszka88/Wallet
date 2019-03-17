using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Wallet.Domain.Repositories;

namespace Wallet.Repositories
{
    public class AccountDataRepository : IAccountDataRepository
    {
        private readonly HttpClient _httpClient;

        public AccountDataRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> GetAll(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri);

            Debug.WriteLine($"Repository.GetAllTransactions | Server response: {(int)response.StatusCode}-{response.StatusCode}");

            if (!response.IsSuccessStatusCode) return null;

            return response;
        }

        public async Task<bool> AddTransaction(IEnumerable<KeyValuePair<string, string>> transaction, Uri uri)
        {
            var postContent = new FormUrlEncodedContent(transaction);

            var response = await _httpClient.PostAsync(uri, postContent);

            Debug.WriteLine($"Repository.AddTransaction | Server response: {(int)response.StatusCode}-{response.StatusCode}");

            return response.IsSuccessStatusCode && response.Content != null;
        }

        public async Task<bool> RemoveTransaction(long transactionId, Uri uri)
        {
            var response = await _httpClient.DeleteAsync(uri);

            Debug.WriteLine($"Repository.RemoveTransaction | Server response: {(int)response.StatusCode}-{response.StatusCode}");

            return response.IsSuccessStatusCode;
        }
    }
}
