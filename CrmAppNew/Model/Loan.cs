using System;
using CrmAppNew.Enums;
using CrmAppNew.Model;

namespace CrmAppNew.Model
{
    sealed public class Loan
    {
        public int Id { get; set; }
        public User User { get; set; } = new User();
        public DateTime DateLoan { get; set; }
        public DateTime RepaymentDate { get; set; }
        public int LoanAmount { get; set; }
        public int LoanBalance { get; set; }
        public LoanType LoanType { get; set; }
        public LoanStatus LoanStatus { get; set; }

        public string Сomment { get; set;} = string.Empty;
    }
}
