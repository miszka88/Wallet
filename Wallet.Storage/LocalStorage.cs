using System.Collections.Generic;
using Wallet.Domain.Services;
using Windows.Storage;

namespace Wallet.Storage
{
    public class LocalStorage : ILocalStorage
    {
        private ApplicationDataContainer _localSettings;
        private StorageFolder _localFolder;

        public LocalStorage()
        {
            SetupLocalEnvironment();
        }

        private void SetupLocalEnvironment()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
            _localFolder = ApplicationData.Current.LocalFolder;
        }

        public void StoreVariable(KeyValuePair<string, string> variable)
        {
            _localSettings.Values[variable.Key] = variable.Value;
        }

        public object ReadVariableValue(string variableName)
        {
            return _localSettings.Values[variableName];
        }
    }
}
