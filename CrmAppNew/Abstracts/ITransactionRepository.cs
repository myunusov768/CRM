using CrmAppNew.Model;
using System;

namespace CrmAppNew.Abstracts
{
    public interface ITransactionRepository
    {
        Result<bool> CreateTransaction(Account debet, Account credit, decimal amount, string description);
    }
}
