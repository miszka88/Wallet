using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wallet.Common.Helpers
{
    public class UriBuilderHelper
    {
        public static Uri BuildUri(string accountAction, string responseTypye, params KeyValuePair<string, string>[] queryParams)
        {
            StringBuilder uri = new StringBuilder(Constants.BaseUri);
            uri.Append(Constants.DefaultPath);
            uri.Append(accountAction);

            if (queryParams.Length > 0)
            {
                if (queryParams.All(x => !string.IsNullOrWhiteSpace(x.Key)))
                {
                    uri.Append(responseTypye);
                }
                else
                {
                    var value = queryParams.Where(param => string.IsNullOrWhiteSpace(param.Key)).Single().Value;
                    uri.Append($"/{value}");
                    uri.Append(responseTypye);
                }

                uri.Append('?');
                uri.Append(string.Join("&", queryParams.Where(param => !string.IsNullOrWhiteSpace(param.Key)).Select(param => $"{param.Key}={param.Value}")));
            }

            return new Uri(uri.ToString());
        }
    }
}
