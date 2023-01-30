using CrmAppNew.Model;
using CrmAppNew.Abstracts;
using CrmAppNew.Enums;

namespace CrmAppNew.AccountCrm
{
    public sealed class AccountService : IAccountService
    {
        private readonly List<Account> _accounts = new List<Account>();

        public Result<Account> GetAccount(decimal numberAccount)
        {
            if (numberAccount is decimal.Zero)
            {
                return new Result<Account> 
                { 
                    IsSuccessfully = false, 
                    Error = Error.OtherErrors, 
                    Message = "Number account is not correct!" 
                };
            }
            var account = _accounts.FirstOrDefault(x=>x.Number.Equals(numberAccount));
            if (account is null) 
            { 
                return new Result<Account> 
                { 
                    IsSuccessfully = false, 
                    Error = Error.OtherErrors, 
                    Message = "Account is not found!" 
                }; 
            }
            return new Result<Account> { IsSuccessfully = true, Message = "Ok!", Payload = account };
        }
        public Result<Account> GetAccount(Guid userId)
        {
            
            var account = _accounts.FirstOrDefault(x => x.UserId.Equals(userId));
            if (account is null)
            {
                return new Result<Account>
                {
                    IsSuccessfully = false,
                    Error = Error.OtherErrors,
                    Message = "Account is not found!"
                };
            }
            return new Result<Account> { IsSuccessfully = true, Message = "Ok!", Payload = account };
        }

        public Result<bool> UpdateBalance(Account account, decimal amount)
        {
            if (account is null)
            {
                return new Result<bool>
                {
                    IsSuccessfully = false,
                    Error = Error.InputParameterIsEmpty,
                    Message = "Input Parameter Is Empty!"
                };
            }
            var accountGet = _accounts.FirstOrDefault(x => x.Number.Equals(account));
            if (account is null)
            {
                return new Result<bool>
                {
                    IsSuccessfully = false,
                    Error = Error.OtherErrors,
                    Message = "Account is not found!"
                };
            }
            return new Result<bool> { IsSuccessfully = true, Message = "Ok!", Payload = true };
        }
    }
}
