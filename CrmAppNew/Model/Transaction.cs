

namespace CrmAppNew.Model
{
    public sealed class Transaction
    {
        public Guid Id { get; set; }
        public Account AccountDebet { get; set; } = new Account();
        public Account AccountCredit { get; set; } = new Account();
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
