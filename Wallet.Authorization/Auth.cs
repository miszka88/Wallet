using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Wallet.Domain.Services;

namespace Wallet.Authorization
{
    public class Auth : IAuthorization
    {
        private readonly IAuthorizationService _authorizationService;

        public Auth(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task Authorize(HttpClient httpClient, IEnumerable<KeyValuePair<string, string>> loginParams, Uri uri)
        {
            await _authorizationService.GetApiKey(httpClient, loginParams, uri);
        }
    }
}
