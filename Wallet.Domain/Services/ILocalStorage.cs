using System.Collections.Generic;

namespace Wallet.Domain.Services
{
    public interface ILocalStorage
    {
        void StoreVariable(KeyValuePair<string, string> variableName);
        object ReadVariableValue(string key);
        void RemoveStoredValue(string key);
    }
}
