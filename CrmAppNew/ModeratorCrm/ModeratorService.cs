using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;

namespace CrmAppNew.ModeratorCrm
{
    sealed class ModeratorService
    {
        private readonly List<User> _users;
        public ModeratorService(List<User> users) { _users = users; }
        private int _id;
        private int CreateID() => _id++;
        public void CreateModerator(CreateUserDto _createUserDto)
        {
            if (_createUserDto == null)
                throw new ArgumentNullException();
            else if (_users.FirstOrDefault(x => x.Login.Equals(_createUserDto.Login) && x.UserRoll.Equals(UserRoll.Moderator)) != null)
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
                    UserRoll = UserRoll.Moderator,
                    Id = CreateID()
                });
        }
        public bool UpdateModerator(CreateUserDto createUserDto, string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals(passwordUser) && x.UserRoll.Equals(UserRoll.Moderator));
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
        public User OpenProfileModer(string loginUser, string passwordUser)
        {
            var admin = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals(passwordUser)
            && x.UserRoll.Equals(UserRoll.Moderator));

            if (admin != null)
                return admin;
            else
                throw new Exception("User's not find:(\n");
        }
        public void DeleteModer(string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(loginUser)
            && x.Password.Equals(passwordUser) && x.UserRoll.Equals(UserRoll.Moderator) && x.moderatorCheck.Equals(ModeratorCheckType.Accept));
            if (user == null)
                throw new Exception("User's not find:(");
            else
                _users.Remove(user);
        }
        public void ModeratorServiceCheck (string login, ModeratorCheckType checkType, string comment) 
        {
            var user = _users.FirstOrDefault(x=> x.Login.Equals(login) && x.moderatorCheck.Equals(ModeratorCheckType.Pending));
            
            if(user == null)
                throw new Exception("User's not find:(\n");
            else if(checkType.Equals(ModeratorCheckType.NotAccept))
            {
                user.moderatorCheck = ModeratorCheckType.NotAccept;
                user.Сomment = comment;
            }
            else if(checkType.Equals(ModeratorCheckType.Accept)) 
            {
                user.moderatorCheck = ModeratorCheckType.Accept;
                user.Сomment = comment;
            }
        }
    }
}
