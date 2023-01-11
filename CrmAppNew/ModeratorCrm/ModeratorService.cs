using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;

namespace CrmAppNew.ModeratorCrm
{
    public sealed class ModeratorService: AbstractUser
    {
        private readonly List<User> _users;
        public ModeratorService(List<User> users) { _users = users; }
        public override Result<bool> СreateUser(CreateUserDto createUserDto)
        {
            var user = _users.FirstOrDefault(x => x.Login != createUserDto.Login && x.Password != createUserDto.Password);
            if (createUserDto is null)
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
                    UserRoll = UserRoll.Moderator,
                    UserStatus = UserStatus.Open
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
            if (createUserDto is null)
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
            else if (user is null)
            {
                var result = new Result<bool>() { Error = Error.UserIsNotFound, IsSuccessfully = false, Message = "User's not found:(!", Payload = false };
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
                var result = new Result<bool>() { IsSuccessfully = true, Message = "User successfully found and change!", Payload = true };
                return result;
            }
        }
        public override Result<User> OpenProfile(string loginUser, string passwordUser)
        {
            var moderator = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals(passwordUser)
            && x.UserRoll.Equals(UserRoll.Moderator));
            if (moderator == null)
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
                    Payload = moderator
                };
                return result;
            }
        }
        public override Result<bool> DeleteUser(Guid userId)
        {
            var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
            if (user is null)
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
        public Result<bool> ModeratorServiceCheck (Guid userId, ModeratorCheckType checkType, string comment) 
        {
            var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
            if (user is null)
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
            else if (checkType.Equals(ModeratorCheckType.NotAccept))
            {
                user.moderatorCheck = ModeratorCheckType.NotAccept;
                user.Сomment = comment;
                var result = new Result<bool>()
                {
                    Error = Error.ModeratorDidNotAcceptYourRequest,
                    IsSuccessfully = false,
                    Message = "The moderator did not accept your request!",
                    Payload = false
                };
                return result;
            }
            else if (checkType.Equals(ModeratorCheckType.Accept))
            {
                user.moderatorCheck = ModeratorCheckType.Accept;
                user.Сomment = comment;
                var result = new Result<bool>()
                {
                    IsSuccessfully = true,
                    Message = "Moderator accepted your request!",
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
                    Message = "Other errors!",
                    Payload = false
                };
                return result;
            }
        }
    }
}
