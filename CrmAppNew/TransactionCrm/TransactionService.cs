using CrmAppNew.Enums;
using CrmAppNew.Abstracts;
using CrmAppNew.Model;
using System.Security.Principal;

namespace CrmAppNew.TransactionCrm
{
    public sealed class TransactionService : ITransactionService
    {
        ITransactionRepository transactionRepository;
        IAccountService accountService;

        public TransactionService(ITransactionRepository transactionRepository, IAccountService accountService)
        {
            this.transactionRepository = transactionRepository;
            this.accountService = accountService;
        }

        public Result<bool> GetCashAtm(decimal accountNumber, decimal amount)
        {
            if (amount <= 0)
            {
                return new Result<bool> { IsSuccessfully = false, Error = Error.AmountZeroOrLessThanZero, Message = "AmountZeroOrLessThanZero!" };
            }

            var account = accountService.GetAccount(accountNumber);
            if (!account.IsSuccessfully)
            {
                return new Result<bool> { IsSuccessfully = false, Error = Error.OtherErrors, Message = "Account is not found!" };
            }
            if (account.Payload.Balance < amount)
            {
                return new Result<bool>
                {
                    IsSuccessfully = false,
                    Error = Error.OtherErrors,
                    Message = "Account balance is less than the amount to be withdrawn!"
                };
            }
            var creditAccount = new Account() { Number = 10103000000000000000 };
            string descriptionTransaction = $"Get cash in Atm {creditAccount.Number}";
            
            transactionRepository.CreateTransaction
                (debet: account.Payload, credit: creditAccount, amount: amount, description: descriptionTransaction);

            return new Result<bool> { IsSuccessfully = true, Message = "Ok", Payload = true };
        }

        public Result<bool> TransferToAccount(decimal debet, decimal credit, decimal amount)
        {
            if (amount <= 0)
            {
                return new Result<bool> { IsSuccessfully = false, Error = Error.AmountZeroOrLessThanZero, Message = "AmountZeroOrLessThanZero!" };
            }

            var debetAccount = accountService.GetAccount(debet);
            var creditAccount = accountService.GetAccount(credit);
            if (!debetAccount.IsSuccessfully)
            {
                return new Result<bool> { IsSuccessfully = false, Error = Error.OtherErrors, Message = "Debet account is not found!" };
            }
            if (!creditAccount.IsSuccessfully)
            {
                return new Result<bool> { IsSuccessfully = false, Error = Error.OtherErrors, Message = "Credit account is not found!" };
            }
            if (debetAccount.Payload.Balance < amount)
            {
                return new Result<bool>
                {
                    IsSuccessfully = false,
                    Error = Error.OtherErrors,
                    Message = "Account balance is less than the amount to be withdrawn!"
                };
            }
            string descriptionTransaction = $"Transfer between {debetAccount.Payload.Number} and {creditAccount.Payload.Number}";
            transactionRepository.CreateTransaction
                (debet: debetAccount.Payload, credit: creditAccount.Payload, amount: amount, description: descriptionTransaction);
            
            return new Result<bool> { IsSuccessfully = true, Message = "Ok", Payload = true };

        }
    }
}
