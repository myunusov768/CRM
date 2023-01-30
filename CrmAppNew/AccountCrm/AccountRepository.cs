
using CrmAppNew.Abstracts;
using CrmAppNew.Enums;
using CrmAppNew.Model;

namespace CrmAppNew.AccountCrm
{
    public sealed class AccountRepository : IAccountRepository
    {
        private readonly List<Account> _accounts;
        private readonly List<User> _users;
        public AccountRepository(List<Account> acounts, List<User> users)
        {
            _accounts = acounts; _users = users;
        }

        private decimal _card = 20216000000000000000M;
        private decimal CreateNewAccount() => _card++;

        public Result<bool> CreateAccount(Guid userId)
        {
            var result = new Result<bool>();
            var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
            if (user is null)
            {
                result.IsSuccessfully = false;
                result.Error = Error.UserIsNotFound;
                result.Message = "User Is Not Found";
                result.Payload = false;
                return result;
            }
            _accounts.Add(new Account()
            {
                Id = Guid.NewGuid(),
                Number = CreateNewAccount(),
                AccountType = AccountType.Card,
                UserId = userId,
                Category = AccountCategory.Passive,
                Balance = 0
            });
            result.IsSuccessfully = true;
            result.Message = "Ok";
            result.Payload = true;
            return result;
        }
    }
}
