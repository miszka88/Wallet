using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Domain.Services;

namespace Wallet.Authorization
{
    public class Authorization : IAuthorization
    {
        public Task<string> GetApiKey(IEnumerable<Tuple<string, string>> loginParams, Uri apiKeyUri)
        {
            throw new NotImplementedException();
            //UriBuilder builder = new UriBuilder(apiKeyUri);
            //builder.Password = loginParams.First(p => p.Item1 == "password").Item2;
            //builder.UserName = loginParams.First(p => p.Item1 == "email").Item2;
            //builder.Fragment = "session";
            //builder.Fragment = ".json";


            //return;
        }
    }
}
