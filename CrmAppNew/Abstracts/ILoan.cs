using CrmAppNew.Model;

namespace CrmAppNew.Abstracts
{
    public interface ILoan
    {
        public abstract Result<bool> RepaymentLoan(Guid idLoan, int amount);
        public abstract Result<bool> CreateLoan(Guid userId, int amountLoan);
        public abstract Result<Loan> GetSpecificLoan(Guid idLoan);
        public abstract Result<string> GetAllLoansUser(Guid userId);
    }
}
