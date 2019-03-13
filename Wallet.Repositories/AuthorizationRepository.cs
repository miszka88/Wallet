using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Wallet.Domain.Repositories;

namespace Wallet.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly HttpClient _httpClient;
        public AuthorizationRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetApiKey(IEnumerable<KeyValuePair<string, string>> loginParams, Uri apiKeyUri)
        {
            var requestContent = new FormUrlEncodedContent(loginParams);

            var response = await _httpClient.PostAsync(apiKeyUri, requestContent);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"{(int)response.StatusCode}|{response.StatusCode}");
                return await response.Content.ReadAsStringAsync();
            }
            Debug.WriteLine(response.StatusCode.ToString());
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new HttpRequestException("User or password is incorrect");
            else
                throw new HttpRequestException(response.StatusCode.ToString());
        }
    }
}
