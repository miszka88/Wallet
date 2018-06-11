using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wallet.Authorization
{
    public interface IAuthorization
    {
        Task Authorize(HttpClient httpClient, IEnumerable<KeyValuePair<string, string>> loginParams, Uri uri);
    }
}
