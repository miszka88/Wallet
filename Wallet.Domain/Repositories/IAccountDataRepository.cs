using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wallet.Domain.Repositories
{
    public interface IAccountDataRepository
    {
        Task<HttpResponseMessage> Get(Uri uri);
    }
}
