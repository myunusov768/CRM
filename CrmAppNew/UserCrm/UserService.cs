using CrmAppNew.DTO;
using CrmAppNew.Model;
using CrmAppNew.UserCRM;
using CrmAppNew.Enums;
using System.Collections;

namespace CrmAppNew.UserCrm
{
    //создание делегат для получение ошибки
    public delegate void MessageError(string message);
    sealed class UserService 
    {
        //Перемен для делегата
        MessageError _message;

        public void RegisterMessage(MessageError message)
        {
            _message = message;
        }

        private readonly List<User> _users;
        //Конструктор для доступ на лист и изменение листа внутри конструктор
        public UserService (List<User> users)
        {
            _users = users;
        }
        private int _id;
        private int CreateID() => _id++;
        //Создать пользователь
        public void СreateUser(CreateUserDto createUserDto)
        {
            var user = _users.FirstOrDefault(x=> x.Login!=createUserDto.Login && x.Password != createUserDto.Password);
            if (createUserDto == null)
                _message.Invoke("Argument Null Exception");
            else if (user != null)
                _message.Invoke("Пользователь с таким логен и пароль уже существует!\n");
            else
            {
                _users.Add(new User()
                {
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    Middlename = createUserDto.Middlename,
                    DateOfBirth = createUserDto.DateOfBirth,
                    Login = createUserDto.Login,
                    Password = createUserDto.Password,
                    UserRoll = UserRoll.User,
                    moderatorCheck = ModeratorCheckType.Pending,
                    Id = CreateID()
                });
            }

        }

        public User GetUser(string userLogin)
        {
            var user = _users.FirstOrDefault(user => user.Login.Equals(userLogin) && user.UserRoll.Equals(UserRoll.User) 
            && user.moderatorCheck.Equals(ModeratorCheckType.Accept));
            if (user == null)
            {
                _message.Invoke("User's not find:(\n");
                return null;
            }
            else
                return user;
        }

        public User OpenProfile(string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x=>x.Login.Equals(loginUser) && x.Password.Equals(passwordUser) 
            && x.UserRoll.Equals(UserRoll.User));

            if(user == null)
            {
                _message.Invoke($"User's not find:(\n");
                return null;
            }
            else if(user.moderatorCheck.Equals(ModeratorCheckType.Accept))
                return user;
            else if (user.moderatorCheck.Equals(ModeratorCheckType.Pending))
            {
                _message.Invoke("Your request for consideration:(\n");
                return null;
            }
            else
            {
                _message.Invoke($"User's not find:(\n");
                return null;
            }
        }
        public bool UserDataChange(CreateUserDto createUserDto, string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals( passwordUser) 
            && x.UserRoll.Equals(UserRoll.User) && x.moderatorCheck.Equals(ModeratorCheckType.Accept));
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
        public void DeleteUser(string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(loginUser) 
            && x.Password.Equals(passwordUser) && x.UserRoll.Equals(UserRoll.User) && x.moderatorCheck.Equals(ModeratorCheckType.Accept));
            if (user == null)
                _message.Invoke("User's not find:(");
            else
                _users.Remove(user);
        }
    }
}
