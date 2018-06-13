using System.Collections.Generic;
using Wallet.Domain.Services;
using Windows.Storage;

namespace Wallet.Storage
{
    public class LocalStorage : ILocalStorage
    {
        private readonly ApplicationDataContainer _localSettings;
        private readonly StorageFolder _localFolder;

        public LocalStorage()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
            _localFolder = ApplicationData.Current.LocalFolder;
        }

        public void StoreVariable(KeyValuePair<string, string> variable)
        {
            _localSettings.Values[variable.Key] = variable.Value;
        }

        public object ReadVariableValue(string key)
        {
            return _localSettings.Values[key];
        }

        public void RemoveStoredValue(string key)
        {
            _localSettings.Values.Remove(key);
        }
    }
}
