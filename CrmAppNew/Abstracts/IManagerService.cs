
using CrmAppNew.Enums;
using CrmAppNew.ManagerCrm;
using CrmAppNew.Model;
using System.Transactions;

namespace CrmAppNew.Abstracts
{
    public interface IManagerService
    {
        public Result<bool> ManagerServiceCheck(Guid id, LoanType checkType, string comment);
    }
}
