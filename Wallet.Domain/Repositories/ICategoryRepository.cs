using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wallet.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<HttpResponseMessage> Get(Uri uri);

    }
}
