using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Domain.Models;

namespace Wallet.Domain.Services
{
    public interface IAccountDataService
    {
        Task<IEnumerable<UserAccountClass>> GetUserAccounts();
        Task<IEnumerable<MoneyTransactionClass>> GetAccountTransactionsById(long accountId);
    }
}
