using CrmAppNew.Enums;
using CrmAppNew.Model;
using CrmAppNew.Abstracts;

namespace CrmAppNew.LoanCrm
{
    public sealed class LoanServise
    {
        private readonly List<Loan> _transactions;
        public LoanServise(List<Loan> loans) { _transactions = loans; }
        private int _tranche = 0;
        private int SetTranche() => ++_tranche;
        public Result<bool> RepaymentLoan(int tranche, int amount)
        {
            var loan = _transactions.FirstOrDefault(x => x.Tranche.Equals(tranche));
            if (loan is null)
            {
                var result = new Result<bool>()
                {
                    Error = Error.LoanIsNotFound,
                    IsSuccessfully = false,
                    Message = "Loan Is Not Found!",
                    Payload = false
                };
                return result;
            }
            else if (amount <= 0)
            {
                var result = new Result<bool>()
                {
                    Error = Error.AmountZeroOrLessThanZero,
                    IsSuccessfully = false,
                    Message = "AmountZeroOrLessThanZero!",
                    Payload = false
                };
                return result;
            }
            else if (amount > loan.LoanAmount)
            {
                var result = new Result<bool>()
                {
                    Error = Error.OtherErrors,
                    IsSuccessfully = false,
                    Message = $"Cумма на погашение больше чем остаток долга по кредиту на сумма {loan.LoanBalance - amount}!",
                    Payload = false
                };
                return result;
            }
            else if (amount >0 && amount < loan.LoanAmount)
            {
                loan.LoanBalance -= amount;
                loan.LoanStatus = LoanStatus.open;
                loan.RepaymentDate = DateTime.Now;
                var result = new Result<bool>()
                {
                    Error = Error.OtherErrors,
                    IsSuccessfully = true,
                    Message = $"OK {loan.LoanBalance}!",
                    Payload = true
                };
                return result;
            }
            else if (amount == loan.LoanAmount)
            {
                loan.LoanBalance -= amount;
                loan.LoanStatus = LoanStatus.close;
                loan.RepaymentDate = DateTime.Now;
                var result = new Result<bool>()
                {
                    Error = Error.OtherErrors,
                    IsSuccessfully = true,
                    Message = $"OK {loan.LoanBalance}!",
                    Payload = true
                };
                return result;
            }
            else 
            {
                var result = new Result<bool>()
                {
                    Error = Error.OtherErrors,
                    IsSuccessfully = false,
                    Message = "Other Errors",
                    Payload = false
                };
                return result;
            }
        }
        public Result<bool> CreateLoan(Guid userId, int amountLoan)
        {
            var user = _transactions.FirstOrDefault(x => x.UserId.Equals(userId));
            if (user == null)
            {
                _transactions.Add(new Loan()
                {
                    Id = Guid.NewGuid(),
                    Tranche = SetTranche(),
                    UserId = userId,
                    LoanAmount = amountLoan,
                    LoanBalance = amountLoan,
                    LoanType = LoanType.Pending,
                    DateLoan = DateTime.Now,
                    RepaymentDate = DateTime.Now.AddMonths(1),
                    LoanStatus= LoanStatus.open,
                });
                var result = new Result<bool>()
                {
                    IsSuccessfully = true,
                    Message = "Ok",
                    Payload = true
                };
                return result;
            }    
            else if (amountLoan <= 0)
            {
                var result = new Result<bool>()
                {
                    Error = Error.AmountZeroOrLessThanZero,
                    IsSuccessfully = false,
                    Message = "Loan amount is null!",
                    Payload = false
                };
                return result;
            }
            else
            {
                if (user.DateLoan.Month + 1 == DateTime.Now.Month)
                {
                    int _loanBalance = user.LoanBalance + amountLoan;
                    _transactions.Add(new Loan()
                    {
                        Id = Guid.NewGuid(),
                        Tranche = SetTranche(),
                        UserId = userId,
                        LoanAmount = amountLoan,
                        LoanBalance = amountLoan,
                        LoanType = LoanType.Pending,
                        DateLoan = DateTime.Now,
                        RepaymentDate = DateTime.Now.AddMonths(1)

                    });
                    var result = new Result<bool>() { IsSuccessfully = true, Message = "Ok", Payload = true };
                    return result;
                }
                else
                {
                    var result = new Result<bool>()
                    {
                        Error = Error.LimitExhausted,
                        IsSuccessfully = false,
                        Message = "Limit Exhausted!",
                        Payload = false
                    };
                    return result;
                }
            }
        }
        public Result<Loan> GetSpecificLoan(int tranche) 
        {
            var loan = _transactions.FirstOrDefault(x => x.Tranche.Equals(tranche));
            if (loan is null)
            {
                var result = new Result<Loan>()
                {
                    Error = Error.LoanIsNotFound,
                    IsSuccessfully = false,
                    Message = "Loan Is Not Found!"
                };
                return result;
            }
            else
            {
                var result = new Result<Loan>()
                {
                    IsSuccessfully = true,
                    Message = "Ok!",
                    Payload = loan
                };
                return result;
            }
        }
        public Result<string> GetAllLoansUser(Guid userId)
        {
            var transactions = _transactions.FirstOrDefault(x => x.UserId.Equals(userId));
            if (transactions is null)
            {
                var result = new Result<string>()
                {
                    Error = Error.OtherErrors,
                    IsSuccessfully = false,
                    Message = "Нou do not have an active loan"
                };
                return result;
            }
            else
            {
                string _loansId = string.Empty;
                foreach (var item in _transactions)
                {
                    if (item.UserId.Equals(userId))
                    {

                        _loansId = _loansId + $"{item.Tranche}, ";
                    }
                }
                var result = new Result<string>()
                {
                    IsSuccessfully = true,
                    Message = "Ok!",
                    Payload = _loansId
                };
                return result;
            }
        }
        public Result<List<Guid>> GetOverdueLoansList()
        {
            if (_transactions is null)
            {
                var result = new Result<List<Guid>>()
                {
                    Error = Error.SheetIsEmpty,
                    IsSuccessfully = false,
                    Message = "SheetIsEmpty!",
                };
                return result;
            }
            else
            {
                var _list = new List<Guid>();
                foreach (var item in _transactions)
                {

                    if (item.LoanStatus.Equals(LoanStatus.open) && item.RepaymentDate.Day - 3 > DateTime.Now.Day)
                        _list.Add(item.UserId);
                }
                var result = new Result<List<Guid>>()
                {
                    IsSuccessfully = true,
                    Message = "Ok!",
                    Payload = _list
                };
                return result;
            }
        }
    }
}
