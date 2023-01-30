
using CrmAppNew.Enums;
using CrmAppNew.Model;

namespace CrmAppNew.Abstracts
{
    public interface IAccountRepository
    {
        Result<bool> CreateAccount(Guid userId);
    }
}
