using CrmAppNew.Abstracts;
using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;
using System.Collections;

namespace CrmAppNew.AdminCrm
{
    public sealed class AdminService: AbstractUser, ILoanService, IManagerService
    {
        private readonly List<Loan> _transactions;
        private readonly List<User> _users;
        public AdminService(List<User> users, List<Loan> transactions) { _users = users; _transactions = transactions; }
        public override Result<bool> СreateUser(CreateUserDto createUserDto)
        {
            var user = _users.FirstOrDefault(x => x.Login != createUserDto.Login && x.Password != createUserDto.Password);
            if (createUserDto is null)
                return new Result<bool> 
                { 
                    Error = Error.InputParameterIsEmpty, 
                    IsSuccessfully = false, 
                    Message = "The user you want to create is empty!", 
                    Payload = false 
                };
            else if (user != null && user.Login.Equals(createUserDto.Login))
                return new Result<bool> 
                {
                    Error = Error.ThisLoginIsAlreadyTaken, 
                    IsSuccessfully = false, 
                    Message = "Login is already taken!", 
                    Payload = false 
                };
            _users.Add(new User()
            {
                Id = Guid.NewGuid(),
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Middlename = createUserDto.Middlename,
                DateOfBirth = createUserDto.DateOfBirth,
                Login = createUserDto.Login,
                Password = createUserDto.Password,
                UserRoll = UserRoll.Admin,
                UserStatus = UserStatus.Open
            });
            var result = new Result<bool>()
            { IsSuccessfully = true, Message = "User successfully created!", Payload = true };
            return result;
        }
        public override Result<bool> UserDataChange(CreateUserDto createUserDto, Guid userId)
        {
            var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
            if (createUserDto is null)
                return new Result<bool>()
                {
                    Error = Error.InputParameterIsEmpty, 
                    IsSuccessfully = false, 
                    Message = "The data user you want to update is empty!", 
                    Payload = false 
                };
            else if (user is null)
                return new Result<bool>()
                {
                    Error = Error.UserIsNotFound, 
                    IsSuccessfully = false, 
                    Message = "User's not found:(!", 
                    Payload = false 
                };
            user.FirstName = createUserDto.FirstName;
            user.LastName = createUserDto.LastName;
            user.Middlename = createUserDto.Middlename;
            user.DateOfBirth = createUserDto.DateOfBirth;
            user.Login = createUserDto.Login;
            user.Password = createUserDto.Password;
            var result = new Result<bool>()
            { IsSuccessfully = true, Message = "User successfully found and change!", Payload = true };
            return result;
        }
        public override Result<bool> DeleteUser(Guid userId)
        {
            var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
            if (user is null)
                return new Result<bool>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!", Payload = false };
            _users.Remove(user);
            var result = new Result<bool>() { IsSuccessfully = true, Message = "User deleted successfully!", Payload = true };
            return result;
        }
        public override Result<User> OpenProfile(string loginUser, string passwordUser)
        {
            var admin = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals(passwordUser)
            && x.UserRoll.Equals(UserRoll.Admin));

            if (admin is null) return new Result<User>()
            { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!" };
            
            var result = new Result<User>()
            { IsSuccessfully = true, Message = "User successfully found!", Payload = admin };
            return result;
        }
        public Result<User> GetSpecificUser(string userLogin)
        {
            var user = _users.FirstOrDefault(user => user.Login.Equals(userLogin) && user.UserRoll.Equals(UserRoll.User));
            if (user is null) return new Result<User>()
            { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!" };
            
            var result = new Result<User>()
            { IsSuccessfully = true, Message = "User successfully found!", Payload = user };
            return result;
        }
        public IEnumerator GetAllUsers()
        {
            foreach (var user in _users) 
                if (user.UserRoll.Equals(UserRoll.User) && _users != null)
                    yield return user;
        }
        /*
 - Добавить Администратору возможность:
 - Редактировать пользователей (всех со всеми ролями).
 - Удалять пользователей (всех со всеми ролями).
 - Блокировать пользователям доступ в СРМ (всех со всеми ролями).
 - Создать долг для определенного пользователя.
 - Подтверждать этот долг.
 - Погошать долги пользователей.*/
        public Result<bool> ChangeStatusUser(string login, UserStatus userStatus)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(login));
            if (user is null)
                return new Result<bool>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!", Payload = false };
            user.UserStatus = userStatus;
            var result = new Result<bool>()
            { IsSuccessfully = true, Message = $"User successfully found and status changes to {userStatus}!", Payload = true };
            return result;
        }
        public Result<bool> DeleteUser(string login)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(login));
            if (user is null)
                return new Result<bool>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!", Payload = false };
            _users.Remove(user);
            var result = new Result<bool>()
            { IsSuccessfully = true, Message = "User successfully found and deleted!", Payload = true };
            return result;
        }
        public Result<bool> UpdateUsers(CreateUserDto createUserDto, string login)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(login));
            if (createUserDto is null)
                return new Result<bool>()
                { Error = Error.InputParameterIsEmpty, IsSuccessfully = false, Message = "The data user you want to update is empty!", Payload = false };
            else if (user is null)
                return new Result<bool>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!", Payload = false };
            user.FirstName = createUserDto.FirstName;
            user.LastName = createUserDto.LastName;
            user.Middlename = createUserDto.Middlename;
            user.DateOfBirth = createUserDto.DateOfBirth;
            user.Login = createUserDto.Login;
            user.Password = createUserDto.Password;
            var result = new Result<bool>()
            { IsSuccessfully = true, Message = "User successfully found and change!", Payload = true };
            return result;
        }
        //------------------------------------------------------------------
        public Result<bool> CreateLoan(Guid userId, int amountLoan)
        {
            var user = _transactions.FirstOrDefault(x => x.UserId.Equals(userId));
            if (user == null)
            {
                _transactions.Add(new Loan()
                {
                    Id = Guid.NewGuid(),
                    Tranche = CreateTrancheLoans.SetTranche(),
                    UserId = userId,
                    LoanAmount = amountLoan,
                    LoanBalance = amountLoan,
                    LoanType = LoanType.Pending,
                    DateLoan = DateTime.Now,
                    RepaymentDate = DateTime.Now.AddMonths(1),
                    LoanStatus = LoanStatus.open,
                });
                var result = new Result<bool>()
                { IsSuccessfully = true, Message = "Ok", Payload = true };
                return result;
            }
            else if (amountLoan <= 0)
            {
                var result = new Result<bool>()
                {Error = Error.AmountZeroOrLessThanZero,IsSuccessfully = false,Message = "Loan amount is null!",Payload = false};
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
                        Tranche = CreateTrancheLoans.SetTranche(),
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
                    {Error = Error.LimitExhausted,IsSuccessfully = false,Message = "Limit Exhausted!",Payload = false};
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
                {Error = Error.LoanIsNotFound,IsSuccessfully = false,Message = "Loan Is Not Found!"};
                return result;
            }
            else
            {
                var result = new Result<Loan>()
                { IsSuccessfully = true, Message = "Ok!", Payload = loan };
                return result;
            }
        }
        public Result<bool> RepaymentLoan(int tranche, int amount)
        {
            var loan = _transactions.FirstOrDefault(x => x.Tranche.Equals(tranche));
            if (loan is null)
            {
                var result = new Result<bool>()
                {Error = Error.LoanIsNotFound,IsSuccessfully = false,Message = "Loan Is Not Found!",Payload = false};
                return result;
            }
            else if (amount <= 0)
            {
                var result = new Result<bool>()
                {Error = Error.AmountZeroOrLessThanZero,IsSuccessfully = false,Message = "AmountZeroOrLessThanZero!",Payload = false};
                return result;
            }
            else if (amount > loan.LoanAmount)
            {
                var result = new Result<bool>(){
                    Error = Error.OtherErrors,IsSuccessfully = false,
                    Message = $"Cумма на погашение больше чем остаток долга по кредиту на сумма {loan.LoanBalance - amount}!",Payload = false};
                return result;
            }
            else if (amount > 0 && amount < loan.LoanAmount)
            {
                loan.LoanBalance -= amount;
                loan.LoanStatus = LoanStatus.open;
                loan.RepaymentDate = DateTime.Now;
                var result = new Result<bool>()
                {Error = Error.OtherErrors,IsSuccessfully = true,Message = $"OK {loan.LoanBalance}!",Payload = true};
                return result;
            }
            else if (amount == loan.LoanAmount)
            {
                loan.LoanBalance -= amount;
                loan.LoanStatus = LoanStatus.close;
                loan.RepaymentDate = DateTime.Now;
                var result = new Result<bool>()
                {Error = Error.OtherErrors,IsSuccessfully = true,Message = $"OK {loan.LoanBalance}!",Payload = true};
                return result;
            }
            else
            {
                var result = new Result<bool>()
                {Error = Error.OtherErrors,IsSuccessfully = false,Message = "Other Errors",Payload = false};
                return result;
            }
        }
        public Result<string> GetAllLoansUser(Guid userId)
        {
            var transactions = _transactions.FirstOrDefault(x => x.UserId.Equals(userId));
            if (transactions is null)
            {
                var result = new Result<string>()
                {Error = Error.OtherErrors,IsSuccessfully = false,Message = "You do not have an active loan"};
                return result;
            }
            else
            {
                string _loansId = string.Empty;
                foreach (var item in _transactions)
                    if (item.UserId.Equals(userId))
                        _loansId = _loansId + $"{item.Tranche}, ";
                var result = new Result<string>()
                {IsSuccessfully = true,Message = "Ok!",Payload = _loansId};
                return result;
            }
        }
        public Result<bool> ManagerServiceCheck(Guid id, LoanType checkType, string comment)
        {
            var loan = _transactions.FirstOrDefault(x => x.Id.Equals(id));
            if (loan is null)
            {
                var result = new Result<bool>()
                {Error = Error.LoanIsNotFound,IsSuccessfully = false,Message = "Loan is not found",Payload = false};
                return result;
            }
            else if (checkType.Equals(LoanType.Accept))
            {
                loan.LoanType = LoanType.Accept;
                loan.Сomment = comment;
                var result = new Result<bool>()
                {IsSuccessfully = true,Message = "Loan successfully approved",Payload = true};
                return result;
            }
            else if (checkType.Equals(LoanType.NotAccept))
            {
                loan.LoanType = LoanType.NotAccept;
                loan.Сomment = comment;
                var result = new Result<bool>()
                {IsSuccessfully = false,Message = "Your loan application has been canceled",Payload = false};
                return result;
            }
            else
            {
                var result = new Result<bool>()
                {Error = Error.OtherErrors,IsSuccessfully = false,Message = "Other errors!",Payload = false};
                return result;
            }
        }
    }

}
