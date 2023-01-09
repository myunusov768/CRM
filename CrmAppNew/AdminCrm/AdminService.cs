using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;
using System.Collections;

namespace CrmAppNew.AdminCrm
{
    public sealed class AdminService: AbstractUser
    {
        private readonly List<User> _users;
        public AdminService(List<User> users) { _users = users; }
        public override Result<bool> СreateUser(CreateUserDto createUserDto)
        {
            var user = _users.FirstOrDefault(x => x.Login != createUserDto.Login && x.Password != createUserDto.Password);
            if (createUserDto == null)
                return new Result<bool> { Error = Error.InputParameterIsEmpty, IsSuccessfully = false, Message = "The user you want to create is empty!", Payload = false };
            else if (user != null && user.Login.Equals(createUserDto.Login))
                return new Result<bool> { Error = Error.ThisLoginIsAlreadyTaken, IsSuccessfully = false, Message = "Login is already taken!", Payload = false };
            _users.Add(new User()
            {
                Id = Guid.NewGuid(),
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Middlename = createUserDto.Middlename,
                DateOfBirth = createUserDto.DateOfBirth,
                Login = createUserDto.Login,
                Password = createUserDto.Password,
                UserRoll = UserRoll.Admin
            });
            var result = new Result<bool>()
            { IsSuccessfully = true, Message = "User successfully created!", Payload = true };
            return result;
        }
        public override Result<bool> UserDataChange(CreateUserDto createUserDto, Guid userId)
        {
            var user = _users.FirstOrDefault(x => x.Id.Equals(userId));
            if (createUserDto == null)
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
    }
}
