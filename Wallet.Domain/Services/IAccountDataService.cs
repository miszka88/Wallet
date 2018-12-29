using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Wallet.Domain.Models;

namespace Wallet.Domain.Services
{
    public interface IAccountDataService
    {
        Task<IEnumerable<UserAccountObject>> GetUserAccountsData();
        Task<IEnumerable<MoneyTransactionObject>> GetTransactionsByAccountId(long accountId);
        Task AddTransaction(MoneyTransaction transaction, long walletId);
        Task<IDictionary<long, string>> GetUserAccountsList();
        Task<IDictionary<long, string>> GetDefaultUserWallet();
        Task<ObservableCollection<GroupedUserAccount>> GetGroupedUserAccounts();
        Task<IEnumerable<GroupedMoneyTransaction>> GetGroupedTransactionsByAccountId(long accountId);
    }
}
