

using CrmAppNew.Model;

namespace CrmAppNew.Abstracts
{
    public interface IAccountService
    {
        Result<Account> GetAccount(decimal numberAccount);
        Result<bool> UpdateBalance(Account account, decimal amount);
        Result<Account> GetAccount(Guid userId);

    }
}
