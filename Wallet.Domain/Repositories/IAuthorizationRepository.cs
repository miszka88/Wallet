using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wallet.Domain.Repositories
{
    public interface IAuthorizationRepository
    {
        Task<string> GetApiKey(IEnumerable<KeyValuePair<string, string>> loginParams, Uri apiKeyUri);
    }
}
