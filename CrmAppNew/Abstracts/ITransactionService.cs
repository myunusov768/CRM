
using CrmAppNew.Model;

namespace CrmAppNew.Abstracts
{
    public interface ITransactionService
    {
        Result<bool> GetCashAtm(decimal accountNumber, decimal amount);
        Result<bool> TransferToAccount(decimal debet, decimal credit, decimal amount);
    }
}
