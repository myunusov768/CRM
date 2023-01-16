using CrmAppNew.Model;

namespace CrmAppNew.Abstracts
{
    public interface ILoanService
    {
        public Result<bool> CreateLoan(Guid userId, int amountLoan);
        public Result<bool> RepaymentLoan(int tranche, int amount);
        
        public Result<Loan> GetSpecificLoan(int tranche);
        public Result<string> GetAllLoansUser(Guid userId);
    }
}
