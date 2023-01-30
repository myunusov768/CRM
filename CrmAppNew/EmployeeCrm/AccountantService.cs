using CrmAppNew.Abstracts;
using CrmAppNew.Enums;
using CrmAppNew.Model;

namespace CrmAppNew.EmployeeCrm
{
    public sealed class AccountantService
    {
        private readonly List<EmployeCompany> _employes;
        private ITransactionRepository _transactionRepository;
        public AccountantService(List<EmployeCompany> employes, ITransactionRepository _transactionRepository)
        {
            _employes = employes;
            this._transactionRepository = _transactionRepository;
        }

        public Result<bool> Payroll()
        {
            if (_employes.Count == 0)
            {
                var result1 = new Result<bool>()
                { Error = Error.SheetIsEmpty, IsSuccessfully = false, Message = "List employee is empty!", Payload = false };
                return result1;
            }
            foreach (var company in _employes) 
            {
                Account accountDebet = new Account() { Number = 26202000000000000000M };
                string descreption = "Salary";
                _transactionRepository.CreateTransaction(accountDebet, company.Account, company.Salary, descreption);
            }
            var result = new Result<bool>()
            { IsSuccessfully = true, Message = "Ok!", Payload = true };
            return result;
        }
        public Result<bool> GetAdvance(decimal summ, Guid employeeId)
        {
            var employe = _employes.FirstOrDefault(x => x.Id.Equals(employeeId));
            if (summ >= 0)
            {
                var result1 = new Result<bool>()
                { Error = Error.AmountZeroOrLessThanZero, IsSuccessfully = false, Message = "List employee is empty!", Payload = false };
                return result1;
            }
            else if (employe is null)
            {
                var result1 = new Result<bool>()
                { Error = Error.InputParameterIsEmpty, IsSuccessfully = false, Message = "Employee is not found!", Payload = false };
                return result1;
            }
            else if (summ > employe.Salary)
            {
                var result1 = new Result<bool>()
                { Error = Error.AdvancePaymentMoreThanSalary, IsSuccessfully = false, Message = "Advance payment more than salary!", Payload = false };
                return result1;
            }
            employe.Advance += summ;
            Account accountDebet = new Account() { Number = 17507000000000000000M };
            string descreption = "Advance";
            _transactionRepository.CreateTransaction(accountDebet, employe.Account, summ, descreption);
            var result = new Result<bool>()
            { IsSuccessfully = true, Message = "Ok!", Payload = true };
            return result;
        }

    }
}
