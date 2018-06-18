using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Common;

namespace Wallet.Domain.Services
{
    public interface ICategoryService
    {
        Task<IDictionary<long, string>> GetAll();
        Task<IDictionary<long, string>> GetByTransactionDirection(TransactionDirection.Type transactionDirectionType);
    }
}
