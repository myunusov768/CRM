using CrmAppNew.Enums;

namespace CrmAppNew.DTO
{
    public sealed class EmployeeDto
    {
        public decimal Salary { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PassportDetails { get; set; } = string.Empty;
    }
}
