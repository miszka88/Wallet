using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wallet.Domain.Repositories
{
    public interface IAccountDataRepository
    {
        Task<HttpResponseMessage> Get(Uri uri);
        Task<bool> AddTransaction(IEnumerable<KeyValuePair<string, string>> transaction, Uri uri);
    }
}
