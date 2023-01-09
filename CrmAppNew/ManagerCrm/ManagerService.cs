using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;

namespace CrmAppNew.ManagerCrm
{
    public sealed class ManagerService: AbstractUser
    {
        private readonly List<User> _users;
        private readonly List<Loan> _transactions;
        public ManagerService(List<User> users, List<Loan> transactions) { _users = users; _transactions = transactions; }        
        public override Result<bool> СreateUser(CreateUserDto createUserDto)
        {
            var user = _users.FirstOrDefault(x => x.Login != createUserDto.Login && x.Password != createUserDto.Password);
            if (createUserDto == null)
            {
                var result = new Result<bool>()
                {
                    Error = Error.InputParameterIsEmpty,
                    IsSuccessfully = false,
                    Message = "The user you want to create is empty!",
                    Payload = false
                };
                return result;
            }
            else if (user != null && user.Login.Equals(createUserDto.Login))
            {

                var result = new Result<bool>()
                {
                    Error = Error.ThisLoginIsAlreadyTaken,
                    IsSuccessfully = false,
                    Message = "Login is already taken!",
                    Payload = false
                };
                return result;
            }
            else
            {
                _users.Add(new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    Middlename = createUserDto.Middlename,
                    DateOfBirth = createUserDto.DateOfBirth,
                    Login = createUserDto.Login,
                    Password = createUserDto.Password,
                    UserRoll = UserRoll.Manager
                });
                var result = new Result<bool>()
                {
                    IsSuccessfully = true,
                    Message = "User successfully created!",
                    Payload = true
                };
                return result;
            }
        }
        public override Result<bool> UserDataChange(CreateUserDto createUserDto, Guid userId)
        {
            var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
            if (createUserDto == null)
            {
                var result = new Result<bool>()
                {
                    Error = Error.InputParameterIsEmpty,
                    IsSuccessfully = false,
                    Message = "The data user you want to update is empty!",
                    Payload = false
                };
                return result;
            }
            else if (user == null)
            {
                var result = new Result<bool>()
                {
                    Error = Error.UserIsNotFound,
                    IsSuccessfully = false,
                    Message = "User's not found:(!",
                    Payload = false
                };
                return result;
            }
            else
            {
                user.FirstName = createUserDto.FirstName;
                user.LastName = createUserDto.LastName;
                user.Middlename = createUserDto.Middlename;
                user.DateOfBirth = createUserDto.DateOfBirth;
                user.Login = createUserDto.Login;
                user.Password = createUserDto.Password;
                var result = new Result<bool>()
                {
                    IsSuccessfully = true,
                    Message = "User successfully found and change!",
                    Payload = true
                };
                return result;
            }
        }
        public override Result<bool> DeleteUser(Guid userId)
        {
            var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
            if (user == null)
            {
                var result = new Result<bool>()
                {
                    Error = Error.UserIsNotFound,
                    IsSuccessfully = false,
                    Message = "User's not found:(!",
                    Payload = false
                };
                return result;
            }
            else
            {
                _users.Remove(user);
                var result = new Result<bool>() { IsSuccessfully = true, Message = "User deleted successfully!", Payload = true };
                return result;
            }
        }
        public override Result<User> OpenProfile(string loginUser, string passwordUser)
        {
            var manager = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals(passwordUser)
            && x.UserRoll.Equals(UserRoll.Moderator));
            if (manager == null)
            {
                var result = new Result<User>()
                {
                    Error = Error.UserIsNotFound,
                    IsSuccessfully = false,
                    Message = "User's not found:(!"
                };
                return result;
            }
            else
            {
                var result = new Result<User>()
                {
                    IsSuccessfully = true,
                    Message = "User successfully found!",
                    Payload = manager
                };
                return result;
            }
        }
        public Result<bool> ManagerServiceCheck(Guid id, LoanType checkType, string comment)
        {
            var loan = _transactions.FirstOrDefault(x => x.Id.Equals(id));
            if (loan == null)
            {
                var result = new Result<bool>()
                {
                    Error = Error.LoanIsNotFound,
                    IsSuccessfully = false,
                    Message = "Loan is not found",
                    Payload = false
                };
                return result;
            }
            else if (checkType.Equals(LoanType.Accept))
            {
                loan.LoanType = LoanType.Accept;
                loan.Сomment = comment;
                var result = new Result<bool>()
                {
                    IsSuccessfully = true,
                    Message = "Loan successfully approved",
                    Payload = true
                };
                return result;
            }
            else if (checkType.Equals(LoanType.NotAccept))
            {
                loan.LoanType = LoanType.NotAccept;
                loan.Сomment = comment;
                var result = new Result<bool>()
                {
                    IsSuccessfully = false,
                    Message = "Your loan application has been canceled",
                    Payload = false
                };
                return result;
            }
            else
            {
                var result = new Result<bool>()
                {
                    Error = Error.OtherErrors,
                    IsSuccessfully = false,
                    Message = "Other errors!",
                    Payload = false
                };
                return result;
            }
        }
    }
}
