using CrmAppNew.Enums;

namespace CrmAppNew.Model
{
    public sealed class Account
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Number { get; set; }
        public int Balance { get; set; }
        public AccountType AccountType { get; set; }
        public AccountCategory Category { get; set; }
    }
}
