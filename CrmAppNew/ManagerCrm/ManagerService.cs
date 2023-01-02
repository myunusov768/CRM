using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.LoanCrm;
using CrmAppNew.Model;
using System;

namespace CrmAppNew.ManagerCrm
{
    sealed class ManagerService
    {
        private readonly List<User> _users;
        private readonly List<Loan> _transactions;
        public ManagerService(List<User> users, List<Loan> transactions) { _users = users; _transactions = transactions; }
        private int _id;
        private int CreateID() => _id++;
        public void CreateManager(CreateUserDto _createUserDto)
        {
            if (_createUserDto == null)
                throw new ArgumentNullException();
            else if (_users.FirstOrDefault(x => x.Login.Equals(_createUserDto.Login) && x.UserRoll.Equals(UserRoll.Manager)) != null)
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
                    UserRoll = UserRoll.Manager,
                    Id = CreateID()
                });
        }
        public bool UpdateManager(CreateUserDto createUserDto, string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals(passwordUser) && x.UserRoll.Equals(UserRoll.Manager));
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
        public void DeleteManager(string loginUser, string passwordUser)
        {
            var user = _users.FirstOrDefault(x => x.Login.Equals(loginUser)
            && x.Password.Equals(passwordUser) && x.UserRoll.Equals(UserRoll.Manager) && x.moderatorCheck.Equals(ModeratorCheckType.Accept));
            if (user == null)
                throw new Exception("User's not find:(");
            else
                _users.Remove(user);
        }
        public User OpenProfileManager(string loginUser, string passwordUser)
        {
            var admin = _users.FirstOrDefault(x => x.Login.Equals(loginUser) && x.Password.Equals(passwordUser)
            && x.UserRoll.Equals(UserRoll.Manager));

            if (admin != null)
                return admin;
            else
                throw new Exception("User's not find:(\n");
        }

        public void ManagerServiceCheck(int id, LoanType checkType, string comment)
        {
            var loan = _transactions.FirstOrDefault(x => x.User.Id.Equals(id) && x.User.moderatorCheck.Equals(ModeratorCheckType.Accept));

            if (loan == null)
                throw new Exception("Loan's not find:(\n");
            else if (checkType.Equals(LoanType.Accept))
            {
                loan.LoanType = LoanType.Accept;
                loan.Сomment = comment;
            }
            else if (checkType.Equals(LoanType.NotAccept))
            {
                loan.LoanType = LoanType.NotAccept;
                loan.Сomment = comment;
            }
        }
        public User GetUser(string userLogin)
        {
            var user = _users.FirstOrDefault(user => user.Login.Equals(userLogin) && user.UserRoll.Equals(UserRoll.Manager)
            && user.moderatorCheck.Equals(ModeratorCheckType.Accept));
            if (user == null)
                throw new Exception("User's not find:(\n");
            else
                return user;
        }
    }
}
