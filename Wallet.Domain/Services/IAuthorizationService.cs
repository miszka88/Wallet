using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wallet.Domain.Services
{
    public interface IAuthorizationService
    {
        Task GetApiKey(HttpClient httpClient, IEnumerable<KeyValuePair<string, string>> loginParams, Uri uri);
    }
}
