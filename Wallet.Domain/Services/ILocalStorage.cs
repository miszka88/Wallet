using System;

namespace Wallet.Domain.Services
{
    public interface ILocalStorage
    {
        void StoreVariable(Tuple<string, string> variable);
        object ReadVariableValue(string variableName);
    }
}
