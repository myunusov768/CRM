using CrmAppNew.Enums;

namespace CrmAppNew.Model
{
    public sealed class EmployeCompany
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Position Position { get; set; }
        public decimal Salary { get; set; }
        public decimal Advance { get; set; }
        public Account Account { get; set; } = new Account();
        public string Address { get; set; } = string.Empty;
        public string PassportDetails { get; set; } = string.Empty;
    }
}
