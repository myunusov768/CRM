using CrmAppNew.Abstracts;
using CrmAppNew.AccountCrm;
using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;

namespace CrmAppNew.EmployeeCrm
{
    public sealed class EmployeeService : IEmployeService
    {
        private readonly List<EmployeCompany> _employes;
        private readonly List<User> _users;
        IAccountRepository accountRepository;
        IAccountService accountService;
        public EmployeeService(List<EmployeCompany> employes, List<User> users, IAccountRepository accountRepository, IAccountService accountService)
        {
            _employes = employes;
            _users = users;
            this.accountRepository = accountRepository;
            this.accountService = accountService;
        }
        public Result<bool> CreateCompanyEmployee(Guid userId, EmployeeDto employeeDto, Position position)
        {
            var _user = _users.FirstOrDefault(x => x.Id == userId);
            if (_user is null)
                return new Result<bool>
                {
                    Error = Error.InputParameterIsEmpty,
                    IsSuccessfully = false,
                    Message = "The user you want to create is empty!",
                    Payload = false
                };
            else if (employeeDto is null)
                return new Result<bool>
                {
                    Error = Error.InputParameterIsEmpty,
                    IsSuccessfully = false,
                    Message = "The employee you want to create is empty!",
                    Payload = false
                };
            accountRepository.CreateAccount(userId);
            var account = accountService.GetAccount(userId).Payload;
            _employes.Add(new EmployeCompany()
            {
                Id = Guid.NewGuid(),
                Address = employeeDto.Address,
                Position = position,
                Salary = employeeDto.Salary,
                PassportDetails = employeeDto.PassportDetails,
                UserId = userId,
                Account = account
            });
            var result = new Result<bool>()
            { IsSuccessfully = true, Message = "Eployee successfully created!", Payload = true };
            return result;
        }
        public Result<bool> DeleteCompanyEmployee(Guid userId)
        {
            var _user = _employes.FirstOrDefault(x => x.Id == userId);
            if (_user is null)
                return new Result<bool>
                {
                    Error = Error.InputParameterIsEmpty,
                    IsSuccessfully = false,
                    Message = "The user you want to create is empty!",
                    Payload = false
                };

            _employes.Remove(_user);
            var result = new Result<bool>()
            { IsSuccessfully = true, Message = "Eployee successfully deleted!", Payload = true };
            return result;
        }
        public (Result<EmployeCompany>, Result<User>) OpenProfile(string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals(passwordUser));
            if (user is null)
            {
                var res1 = new Result<EmployeCompany>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!" };
                var res2 = new Result<User>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!" };
                return (res1, res2);
            }

            var employee = _employes.FirstOrDefault(x => x.UserId.Equals(user.Id));

            if (employee is null) 
            {
                var res1 = new Result<EmployeCompany>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "Employee's not found:(!" };
                var res2 = new Result<User>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "Employee's not found:(!" };
                return (res1, res2);
            }

            var result = new Result<EmployeCompany>()
            { IsSuccessfully = true, Message = "Employee successfully found!", Payload = employee };
            var result1 = new Result<User>()
            { IsSuccessfully = true, Message = "User successfully found!", Payload = user };
            return (result, result1);
        }
    }
}
