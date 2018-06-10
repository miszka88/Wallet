﻿using System.Collections.Generic;

namespace Wallet.Domain.Services
{
    public interface ILocalStorage
    {
        void SetupLocalEnvironment();
        void StoreVariable(KeyValuePair<string, string> variable);
        object ReadVariableValue(string variableName);
    }
}
