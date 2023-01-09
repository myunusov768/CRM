using CrmAppNew.DTO;
using CrmAppNew.Model;
using CrmAppNew.Enums;

namespace CrmAppNew.UserCrm
{

    public sealed class UserService : AbstractUser
    {
        private readonly List<User> _users;
        //Конструктор для доступ на лист и изменение листа внутри конструктор
        public UserService (List<User> users)
        {
            _users = users;
        }
        //Создать пользователь
        public override Result<bool> СreateUser(CreateUserDto createUserDto)
        {
            var user = _users.FirstOrDefault(x => x.Login != createUserDto.Login && x.Password != createUserDto.Password);
            if (createUserDto is null)
            {
                var result = new Result<bool>()
                { Error = Error.InputParameterIsEmpty, IsSuccessfully = false, Message = "The user you want to create is empty!", Payload = false };
                return result;
            }
            else if (user != null && user.Login.Equals(createUserDto.Login))
            {
                var result = new Result<bool>() 
                { Error = Error.ThisLoginIsAlreadyTaken, IsSuccessfully = false, Message = "Login is already taken!", Payload = false };
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
                    UserRoll = UserRoll.User,
                    moderatorCheck = ModeratorCheckType.Pending
                });
                var result = new Result<bool>()
                { IsSuccessfully = true, Message = "User successfully created!", Payload = true };
                return result;
            }
        }
        public Result<User> GetUser(Guid userId)
        {
            var user = _users.FirstOrDefault(user => user.Id.Equals(userId));
            if (user is null)
            {
                var result = new Result<User>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!" };
                return result;
            }
            else
            {
                var result = new Result<User>()
                { IsSuccessfully = false, Message = "User successfully found!", Payload = user };
                return result;
            }   
        }
        public override Result<User> OpenProfile(string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x=>x.Login.Equals(loginUser) && x.Password.Equals(passwordUser) 
            && x.UserRoll.Equals(UserRoll.User));
            if (user is null)
            {
                var result = new Result<User>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!" };
                return result;
            }

            else if (user.moderatorCheck.Equals(ModeratorCheckType.Pending))
            {
                var result = new Result<User>()
                { Error = Error.YourRequestForConsideration, IsSuccessfully = false, Message = "Your request for consideration:(!" };
                return result;
            }
            else if (user.moderatorCheck.Equals(ModeratorCheckType.NotAccept))
            {
                var result = new Result<User>()
                { Error = Error.YourRequestHasBeenDenied, IsSuccessfully = false, Message = "Your request has been denied:(!" };
                return result;
            }
            else
            {
                var result = new Result<User>()
                { IsSuccessfully = true, Message = "User successfully found!", Payload = user };
                return result;
            }
        }
        public override Result<bool> UserDataChange(CreateUserDto createUserDto, Guid userId)
        {
            var user = _users.FirstOrDefault(x => x.Id.Equals(userId) && x.UserRoll.Equals(UserRoll.User) 
            && x.moderatorCheck.Equals(ModeratorCheckType.Accept));
            if (createUserDto == null)
            {
                var result = new Result<bool>()
                { Error = Error.InputParameterIsEmpty, IsSuccessfully = false, Message = "The data user you want to update is empty!", Payload = false };
                return result;
            }
            else if(user is null)
            {
                var result = new Result<bool>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!", Payload = false };
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
                { IsSuccessfully = true, Message = "User successfully found!", Payload = true };
                return result;
            }
        }
        public override Result<bool> DeleteUser(Guid userId)
        {
            var user = _users.FirstOrDefault(x => x.Id.Equals(userId) 
            && x.UserRoll.Equals(UserRoll.User) && x.moderatorCheck.Equals(ModeratorCheckType.Accept));
            if (user is null)
            {
                var result1 = new Result<bool>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!", Payload = false };
                return result1;
            }
            _users.Remove(user);
            var result = new Result<bool>() { IsSuccessfully = true, Message = "User deleted successfully!", Payload = true };
            return result;
        }
        public Result<User> GetUserWithLogin(string userLogin)
        {
            var user = _users.FirstOrDefault(user => user.Login.Equals(userLogin) && user.UserRoll.Equals(UserRoll.User));
            if (user is null)
            {
                var result1 = new Result<User>()
                { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!" };
                return result1;
            }
            var result = new Result<User>()
            { IsSuccessfully = false, Message = "User successfully found!", Payload = user };
            return result;
        }
    }
}
