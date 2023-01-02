using CrmAppNew.UserCrm;
using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;
using System.Collections;

namespace CrmAppNew.AdminCrm
{
    sealed class AdminService
    {
        private readonly List<User> _users;
        public AdminService(List<User> users) 
        {
            _users = users;
        }
        private int _id;
        private int CreateID() => _id++;
        public void CreateAdmin(CreateUserDto _createUserDto)
        {
            if (_createUserDto == null)
                throw new ArgumentNullException();
            else if(_users.FirstOrDefault(x => x.Login.Equals(_createUserDto.Login) && x.UserRoll.Equals(UserRoll.Admin)) != null)
                throw new Exception("Пользователь с таким логен и пароль уже существует!\n");
            else
                _users.Add(new User()
                {
                    FirstName = _createUserDto.FirstName,
                    LastName = _createUserDto.LastName,
                    Middlename = _createUserDto.Middlename,
                    DateOfBirth = _createUserDto.DateOfBirth,
                    Login = _createUserDto.Login,
                    Password = _createUserDto.Password,
                    UserRoll = UserRoll.Admin,
                    Id = CreateID()
                });
        }
        
        public bool UpdateAdmin(CreateUserDto createUserDto, string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals(passwordUser) && x.UserRoll.Equals(UserRoll.Admin));
            if (createUserDto == null || user == null)
                return false;
            else
            {
                user.FirstName = createUserDto.FirstName;
                user.LastName = createUserDto.LastName;
                user.Middlename = createUserDto.Middlename;
                user.DateOfBirth = createUserDto.DateOfBirth;
                user.Login = createUserDto.Login;
                user.Password = createUserDto.Password;
                return true;
            }
        }
        public void DeleteAdmin(string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(loginUser)
            && x.Password.Equals(passwordUser) && x.UserRoll.Equals(UserRoll.Admin) && x.moderatorCheck.Equals(ModeratorCheckType.Accept));
            if (user == null)
                throw new Exception("User's not find:(");
            else
                _users.Remove(user);
        }
        public User OpenProfileAdmin(string loginUser, string passwordUser)
        {
            var admin = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals(passwordUser)
            && x.UserRoll.Equals(UserRoll.Admin));

            if (admin != null)
                return admin;
            else
                throw new Exception("User's not find:(\n");
        }
        public User GetSpecificUser(string userLogin)
        {
            var user = _users.FirstOrDefault(user => user.Login.Equals(userLogin) && user.UserRoll.Equals(UserRoll.Admin));
            if (user == null)
                throw new Exception("User's not find:(\n");
            else
                return user;
        }
        public IEnumerator GetAllUsers()
        {
            foreach (var user in _users)
            {
                if (user.UserRoll.Equals(UserRoll.User) && user != null)
                    yield return user;
            }
        }
    }
}
