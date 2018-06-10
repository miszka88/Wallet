using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wallet.Common;

namespace Wallet.Services.Utils
{
    public class UriBuilderHelper
    {
        public static Uri BuildUri(string accountAction, string responseTypye, params KeyValuePair<string, string>[] queryParams)
        {
            StringBuilder uri = new StringBuilder(Constants.BaseUri);
            uri.Append(Constants.DefaultPath);
            uri.Append(accountAction);
            uri.Append(responseTypye);

            if (queryParams != null)
            {
                uri.Append('?');
                uri.Append(string.Join("&", queryParams.Where(param => !string.IsNullOrWhiteSpace(param.Key)).Select(param => $"{param.Key}={param.Value}")));
            }

            return new Uri(uri.ToString());
        }
    }
}
