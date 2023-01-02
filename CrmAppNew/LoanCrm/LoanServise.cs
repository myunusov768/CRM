using CrmAppNew.Enums;
using CrmAppNew.Model;
using System;
using System.Security.Cryptography.X509Certificates;

namespace CrmAppNew.LoanCrm
{
    sealed class LoanServise
    {
        private readonly List<Loan> _transactions;
        public LoanServise(List<Loan> loans) { _transactions = loans; }

        private int _loanId = 0;
        
        private int SetLoanId() => _loanId++;
        public void RepaymentLoan(int idLoan, int amount)
        {
            var loan = _transactions.FirstOrDefault(x => x.Id.Equals(idLoan));
            if (loan == null)
                throw new ArgumentException("User's not find:(\n");
            else if (amount <= 0)
                throw new ArgumentException("Loan amount is null!\n");
            else if (loan.LoanAmount < amount)
                throw new ArgumentException($"Cумма на погашение больше чем остаток долга по кредиту, summ {loan.LoanBalance - amount}\n");
            else if (amount == loan.LoanAmount)
            {
                loan.LoanBalance -= amount;
                loan.LoanStatus = LoanStatus.close;
                loan.RepaymentDate = DateTime.Now;
            }
                
        }
        public void CreateLoan(User userLoan, int amountLoan)
        {
            var user = _transactions.FirstOrDefault(x => x.User.Login.Equals(userLoan.Login));
            if (user == null)
            {
                _transactions.Add(new Loan() 
                {
                    User = new User() 
                    { 
                        FirstName = userLoan.FirstName,
                        LastName = userLoan.LastName, 
                        Middlename = userLoan.Middlename, 
                        DateOfBirth = userLoan.DateOfBirth, 
                        Login = userLoan.Login, 
                        Password = userLoan.Password },
                    Id = SetLoanId(),
                    LoanAmount = amountLoan,
                    LoanBalance = amountLoan, 
                    LoanType = LoanType.Pending, 
                    DateLoan = DateTime.Now 
                });
            }    
            else if (amountLoan <= 0)
                throw new ArgumentException("Loan amount is null!\n");
            else
            {
                if (user.DateLoan.Month + 1 == DateTime.Now.Month)
                {
                    int _loanBalance = user.LoanBalance + amountLoan;
                    _transactions.Add(new Loan() { User = new User() { Login = userLoan.Login }, Id = SetLoanId(), LoanAmount = amountLoan, LoanBalance = _loanBalance, LoanType = LoanType.Pending });
                }
                else
                    throw new Exception("Your app limit has been reached!\n");
            }
        }
        public Loan GetSpecificLoan(int idLoan) 
        {
            var loan = _transactions.FirstOrDefault(x => x.Id.Equals(idLoan));
            if(loan == null)
                throw new ArgumentException("User's not find:(\n");
            else
                return loan;
        }
        public string GetAllLoansUser(User user)
        {
            var transactions = _transactions.FirstOrDefault(x => x.User.Login.Equals(user.Login));
            if (transactions == null)
                throw new ArgumentException("Transactions's not find:(\n");
            else if (user == null)
                throw new ArgumentException("Incoming user is empty:(\n");
            else
            {
                string _loansId = string.Empty;
                foreach (var item in _transactions)
                {
                    if(item.User.Login.Equals(user.Login))
                    {

                        _loansId = _loansId + $"{item.Id}, " ;
                    }
                }
                return _loansId;
            }
        }
    }
}
