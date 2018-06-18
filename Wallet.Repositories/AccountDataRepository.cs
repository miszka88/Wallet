﻿using System;
using System.Collections.Generic;
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

            Debug.WriteLine($"Repository.Get() | Server response: {(int)response.StatusCode}-{response.StatusCode}");

            if (!response.IsSuccessStatusCode) return null;

            return response;
        }

        public async Task<bool> AddTransaction(IEnumerable<KeyValuePair<string, string>> transaction, Uri uri)
        {
            var postContent = new FormUrlEncodedContent(transaction);

            var response = await _httpClient.PostAsync(uri, postContent);

            Debug.WriteLine($"Repository.AddTransaction() | Server response: {(int)response.StatusCode}-{response.StatusCode}");

            return response.IsSuccessStatusCode && response.Content != null;
        }
    }
}
