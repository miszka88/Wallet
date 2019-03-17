using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Wallet.Domain.Models;
using Wallet.Domain.Repositories;
using Wallet.Domain.Services;


namespace Wallet.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly ILocalStorage _localStorage;

        public AuthorizationService(IAuthorizationRepository authorizationRepository, ILocalStorage localStorage)
        {
            _authorizationRepository = authorizationRepository;
            _localStorage = localStorage;
        }

        public async Task GetApiKey(HttpClient httpClient, IEnumerable<KeyValuePair<string, string>> loginParams, Uri uri)
        {
            var externalApiResponse = await _authorizationRepository.GetApiKey(loginParams, uri);
            if (!string.IsNullOrWhiteSpace(externalApiResponse))
            {
                var apiKey = JsonConvert.DeserializeObject<UserClass>(externalApiResponse).User.ApiKey;

                _localStorage.StoreVariable(new KeyValuePair<string, string>("ApiKey", apiKey));
            }
        }
    }
}
