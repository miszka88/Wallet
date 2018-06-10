using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wallet.Domain.Services
{
    public interface IAuthorization
    {
        Task<string> GetApiKey(IEnumerable<Tuple<string, string>> loginParams, Uri apiKeyUri);
    }
}
