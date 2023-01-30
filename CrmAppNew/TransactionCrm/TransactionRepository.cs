using CrmAppNew.Abstracts;
using CrmAppNew.Model;
using CrmAppNew.Enums;

namespace CrmAppNew.TransactionCrm
{
    public sealed class TransactionRepository : ITransactionRepository
    {
        private readonly List<Transaction> _transactions = new List<Transaction>();
        public Result<bool> CreateTransaction(Account debet, Account credit, decimal amount, string description)
        {
            if (debet is null || credit is null)
            {
                return new Result<bool>()
                {
                    IsSuccessfully = false,
                    Error = Error.InputParameterIsEmpty,
                    Message = "Debet and credit account argument is null!"
                };
            }
            if (amount <= 0)
            {
                return new Result<bool>()
                {
                    IsSuccessfully = false,
                    Error = Error.AmountZeroOrLessThanZero,
                    Message = "AmountZeroOrLessThanZero"
                };
            }
            if (description is null)
            {
                return new Result<bool>()
                {
                    IsSuccessfully = false,
                    Error = Error.InputParameterIsEmpty,
                    Message = "Decraptoin is enpty"
                };
            }
            _transactions.Add(new Transaction() 
            { 
                Id = Guid.NewGuid(), 
                AccountDebet = debet, 
                AccountCredit = credit, 
                Amount = amount, 
                Description = description 
            });
            return new Result<bool>()
            {
                IsSuccessfully = true,
                Message = "Transactoin is successfuly created!",
                Payload = true
            };
        }
    }
}
