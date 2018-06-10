using System;
using Wallet.Domain.Services;

namespace Wallet.Storage
{
    public class LocalStorage : ILocalStorage
    {
        public object ReadVariableValue(string variableName) => throw new NotImplementedException();
        public void StoreVariable(Tuple<string, string> variable) => throw new NotImplementedException();
    }
}
