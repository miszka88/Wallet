using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Domain.Models;

namespace Wallet.Domain.Services
{
    public interface IAccountDataService
    {
        Task<IEnumerable<UserAccountClass>> GetUserAccountsData();
        Task<IEnumerable<MoneyTransactionClass>> GetAccountTransactionsById(long accountId);
        Task AddTransaction(MoneyTransaction transaction, long walletId);
        Task<IDictionary<long, string>> GetUserAccountsList();
        Task<IDictionary<long, string>> GetDefaultUserWallet();
    }
}
