using CrmAppNew.UserCrm;
using CrmAppNew.DTO;
using CrmAppNew.Model;
using CrmAppNew.AdminCrm;
using CrmAppNew.ModeratorCrm;
using CrmAppNew.ManagerCrm;
using CrmAppNew.LoanCrm;
using CrmAppNew.Enums;
using CrmAppNew.MessageCrm;

namespace CrmAppNew
{
    partial class Program
    {
        static List<Loan> _transactions = new List<Loan>();
        static List<Message> _message = new List<Message>();
        static readonly private List<User> _usersList = new List<User>();
        static UserService userService = new UserService(_usersList);
        static AdminService adminService = new AdminService(_usersList);
        static ModeratorService moderatorService = new ModeratorService(_usersList);
        static ManagerService managerService = new ManagerService(_usersList, _transactions);
        static LoanServise loanService = new LoanServise(_transactions);
        static MessageService messageService = new MessageService(_message);
        static void Main(string[] args)
        {
            userService.RegisterMessage(PrintErrorMessage);
            int i = 0;
            while (i++ < 100)
            {
                try
                {
                    Console.Write("Iput comand:> ");
                    string command = Console.ReadLine();
                    if (command.ToLower().Equals("registruser"))
                        CreateUser();
                    else if (command.ToLower().Equals("openuser"))
                        User();
                    else if (command.ToLower().Equals("regisrtadmin"))
                        CreateAdmin();
                    else if (command.ToLower().Equals("openadmin"))
                        Admin();
                    else if (command.ToLower().Equals("regisrtmoder"))
                        CreateModerator();
                    else if (command.ToLower().Equals("openmoder"))
                        Moderator();
                    else if (command.ToLower().Equals("regisrtmanag"))
                        CreateManager();
                    else if (command.ToLower().Equals("openmanag"))
                        Manager();
                    else if (command.ToLower().Equals("exit"))
                        return;
                    else
                        throw new Exception("Команда {command} некорректно");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }
            Console.WriteLine();

            Console.ReadLine();
        }

        

        #region Manager
        public static void Manager()
        {
            var user = OpenProfileManager();
            int i = 0;
            while (i++ < 100)
            {
                Console.Write("Iput comand Manager:> ");
                string command = Console.ReadLine();

                if (command.ToLower().Equals("start checking"))
                    LoanManagerCheck();
                else if (command.ToLower().Equals("statistic"))
                    CheckingStatistic();
                else if (command.ToLower().Equals("exit"))
                    return;
                else
                    throw new Exception("Команда {command} некорректно");
            }

        }
        public static User OpenProfileManager()
        {
            var user = managerService.OpenProfileManager(LoginInput(), PasswordInput());
            Console.WriteLine($"Welcome {user.FirstName} {user.LastName} {user.Middlename}!!!\n");
            return user;
        }
        
        public static void LoanManagerCheck()
        {

            foreach (var item in _transactions)
            {
                if (item.LoanType.Equals(LoanType.Pending))
                {
                    Console.WriteLine($"FLM: {item.User.FirstName} {item.User.LastName} {item.User.Middlename}" +
                        $"\nDate of loan: {item.DateLoan}, Amount: {item.LoanAmount}, Loan balance {item.LoanBalance}\n");
                    Console.WriteLine("Iput comand:> + or -");
                    string command = Console.ReadLine();
                    Console.WriteLine("Iput comment please!");
                    string comment = Console.ReadLine();
                    if (command.ToLower().Equals("+"))
                        if (comment != null)
                            managerService.ManagerServiceCheck(item.Id, LoanType.Accept, comment);
                        else
                            throw new Exception("Команда {command} некорректно");
                    else if (command.ToLower().Equals("-"))
                        if (comment != null)
                            managerService.ManagerServiceCheck(item.Id, LoanType.NotAccept, comment);
                        else
                            throw new Exception("Команда {command} некорректно");
                    else if (command.ToLower().Equals("exit"))
                        break;
                    else
                        throw new Exception("Команда {command} некорректно");
                }
            }
        }

        public static void CreateManager()
        {
            var user = СreateUserInput();
            managerService.CreateManager(new CreateUserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Middlename = user.Middlename,
                DateOfBirth = user.DateOfBirth,
                Login = user.Login,
                Password = user.Password,
            });
        }

        public static void CheckingStatistic()
        {
            StatisticService statisticService = new StatisticService(_usersList);
            statisticService.CalculationStatistic();
        }


        #endregion

        #region Moderator
        public static void Moderator()
        {
            var user = OpenProfileModerator();
            int i = 0;
            while (i++ < 100)
            {
                Console.Write("Iput comand Moderator:> ");
                string command = Console.ReadLine();
                
                if (command.ToLower().Equals("start checking"))
                    ModeratorProseccChekc();
                else if (command.ToLower().Equals("exit"))
                    return;
                else
                    throw new Exception("Команда {command} некорректно");
            }
            
        }
        public static void ModerDataChange(string loginUser, string passwordUser)
        {
            var user = СreateUserInput();
            bool ff = moderatorService.UpdateModerator(new CreateUserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Middlename = user.Middlename,
                DateOfBirth = user.DateOfBirth,
                Login = user.Login,
                Password = user.Password,
            }, loginUser, passwordUser);
            if (ff)
                Console.WriteLine("Данные успешно изменено!");
            else
                Console.WriteLine("Упс:(");
        }
        public static User OpenProfileModerator()
        {
            var user = moderatorService.OpenProfileModer(LoginInput(), PasswordInput());
            Console.WriteLine($"Welcome {user.FirstName} {user.LastName} {user.Middlename}!!!\n");
            return user;
        }
        public static void CreateModerator()
        {
            var user = СreateUserInput();
            moderatorService.CreateModerator(new CreateUserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Middlename = user.Middlename,
                DateOfBirth = user.DateOfBirth,
                Login = user.Login,
                Password = user.Password,
            });
        }
        public static void ModeratorProseccChekc()
        {
            
            foreach (var item in _usersList)
            {
                if (item.moderatorCheck.Equals(ModeratorCheckType.Pending))
                {
                    Console.WriteLine($"FLM: {item.FirstName} {item.LastName} {item.Middlename}" +
                        $"\nDate of birth: {item.DateOfBirth}, Age: {item.Age}\n");
                    Console.WriteLine("Iput comand:> + or -");
                    string command = Console.ReadLine();
                    Console.WriteLine("Iput comment please!");
                    string comment = Console.ReadLine();
                    if (command.ToLower().Equals("+"))
                        if (comment != null)
                            moderatorService.ModeratorServiceCheck(item.Login, ModeratorCheckType.Accept, comment);
                        else
                            throw new Exception("Команда {command} некорректно");
                    else if (command.ToLower().Equals("-"))
                        if (comment != null)
                            moderatorService.ModeratorServiceCheck(LoginInput(), ModeratorCheckType.NotAccept, comment);
                        else
                            throw new Exception("Команда {command} некорректно");
                    else if (command.ToLower().Equals("exit"))
                        break;
                    else
                        throw new Exception("Команда {command} некорректно");
                }
            }
        }
        #endregion

        #region Admin
        public static void Admin()
        {
            var user = OpenProfileAdmin();
            int i = 0;
            while (i++ < 10)
            {
                Console.WriteLine("Iput comand:> ");
                string command = Console.ReadLine();
                if (command.ToLower().Equals("Get specific user"))
                    adminService.GetSpecificUser( LoginInput());
                else if (command.ToLower().Equals("Get All Users"))
                    adminService.GetAllUsers();
                else if (command.ToLower().Equals("Update Admin"))
                    AdminDataChange(LoginInput(), PasswordInput());
                else if (command.ToLower().Equals("exit"))
                    return;
                else
                    throw new Exception("Команда {command} некорректно");
            }
        }

        public static void CreateAdmin()
        {

            var user = СreateUserInput();
            adminService.CreateAdmin(new CreateUserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Middlename = user.Middlename,
                DateOfBirth = user.DateOfBirth,
                Login = user.Login,
                Password = user.Password,
            });
        }
        public static User OpenProfileAdmin()
        {
            var user = adminService.OpenProfileAdmin(LoginInput(), PasswordInput());
            Console.WriteLine($"Welcome {user.FirstName} {user.LastName} {user.Middlename}!!!\n");
            return user;
        }
        public static void AdminDataChange(string loginUser, string passwordUser)
        {
            var user = СreateUserInput();
            bool ff = adminService.UpdateAdmin(new CreateUserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Middlename = user.Middlename,
                DateOfBirth = user.DateOfBirth,
                Login = user.Login,
                Password = user.Password,
            }, loginUser, passwordUser);
            if (ff)
                Console.WriteLine("Данные успешно изменено!");
            else
                Console.WriteLine("Упс:(");
        }
        #endregion

        #region User
        public static void User()
        {
            var user = OpenProfile();
            if (user == null)
                Console.WriteLine();
            else
            {
                int i = 0;
                while (i++ < 100)
                {
                    Console.Write("Iput comand User:> ");
                    string command = Console.ReadLine();
                    if (command.ToLower().Equals("seeprofile"))
                        SeeProfile(user);
                    else if (command.ToLower().Equals("change"))
                        UserDataChange(user.Login, user.Password);
                    else if (command.ToLower().Equals("create loan"))
                        RequestLoan(user);
                    else if (command.ToLower().Equals("loans"))
                        AllLoans(user);
                    else if (command.ToLower().Equals("messanger"))
                        MessageProsses(user);
                    
                    else if (command.ToLower().Equals("exit"))
                        return;
                    else
                        throw new Exception("Команда {command} некорректно");
                }
            }
        }

        public static void CreateUser()
        {

            var user = СreateUserInput();
            userService.СreateUser(new CreateUserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Middlename = user.Middlename,
                DateOfBirth = user.DateOfBirth,
                Login = user.Login,
                Password = user.Password,
            });
        }
        public static User OpenProfile() 
        {
            var user = userService.OpenProfile(LoginInput(), PasswordInput());
            Console.WriteLine($"Welcome {user.FirstName} {user.LastName} {user.Middlename}!!!\n");
            return user;
        }

        public static void SeeProfile(User user)
        {
            Console.WriteLine($"FLM: {user.FirstName} {user.LastName} {user.Middlename}\nDate of birth: {user.DateOfBirth}, Age: {user.Age}\n");
        }
        public static void UserDataChange( string loginUser, string passwordUser) 
        {
            var user = СreateUserInput();
            bool ff = userService.UserDataChange(new CreateUserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Middlename = user.Middlename,
                DateOfBirth = user.DateOfBirth,
                Login = user.Login,
                Password = user.Password,
            },loginUser,passwordUser);
            if (ff)
                Console.WriteLine("Данные успешно изменено!");
            else
                Console.WriteLine("Упс:(");
        }
        public static void RequestLoan(User user)
        {
            Console.WriteLine("Введите сумма кредита!");
            int amount = int.Parse(Console.ReadLine());
            loanService.CreateLoan(user, amount);

        }
        public static void AllLoans(User user)
        {
            string loans = string.Empty;
            loans = loanService.GetAllLoansUser(user);
            if(loans.Equals(null))
                Console.WriteLine("кредитов нет!");
            else
                Console.WriteLine(loans);
        }
        public static void RepaymentLoan(User user)
        {
            Console.WriteLine("Введите Id транша!");
            int _id = int.Parse(Console.ReadLine());
            var loan = loanService.GetSpecificLoan(_id);
            if (loan != null)
                Console.Write($"Сумма: {loan.LoanAmount}");
            else
                Console.WriteLine("Не правельный транш!");
        }
        #endregion



        public static void MessageProsses(User user)
        {
            int i = 0;
            while (i++ < 100)
            {
                Console.Write("Iput comand Messenger:> ");
                string command = Console.ReadLine();
                if (command.ToLower().Equals("openchat"))
                    OpenChat(user);
                else if (command.ToLower().Equals("exit"))
                    return;
                else
                    throw new Exception("Команда {command} некорректно");
            }
        }

        public static void SentMessage(User sender, User recipient)
        {
            var message = CreateMessage(sender: sender, recipient: recipient);
            var result = messageService.CreateMessage(message);
            Console.WriteLine(result.Error);
        }

        public static void OpenChat(User user)
        {
            var recipient =  RecipientUser(LoginInput());
            if (recipient == null)
                throw new Exception("Recipient isn't found!");
            else
            {
                int i = 0;
                while (i++ < 100)
                {
                    Console.Write("Input comand Messenger:> ");
                    string command = Console.ReadLine();
                    if (command.ToLower().Equals("sendmessage"))
                        CreateMessage(sender: user, recipient: recipient);
                    if (command.ToLower().Equals("exit"))
                        return;
                    else
                        throw new Exception("Команда {command} некорректно\n");


                }
            }
        }

        

        public static void GetMessages(User sender, User recipient)
        {
            var _message = messageService.GetMessages(sender: sender,recipient: recipient);
            foreach (var item in _message)
                Console.WriteLine($"{item.MessageId}- Date: {item.Date}, Message: {item.MessageText}");
        }



        

        public static CreateUserDto СreateUserInput()
        {
            CreateUserDto createUser = new CreateUserDto();
            Console.Write("First Name: ");
            string inputFirstName = Console.ReadLine();
            string firstName = string.Empty;
            if (inputFirstName != null && inputFirstName.Length <= 20)
                createUser.FirstName = inputFirstName;
            else
                throw new Exception($"First Name is null!");

            Console.Write("Last Name: ");
            string inputLastName = Console.ReadLine();
            string lastName = string.Empty;
            if (inputLastName != null && inputLastName.Length <= 20)
                createUser.LastName = inputLastName;
            else
                throw new Exception("Last Name is null!");

            Console.Write("Middlename: ");
            string inputMiddlename = Console.ReadLine();
            string middlename = string.Empty;
            if (inputMiddlename != null && inputMiddlename.Length <= 20)
                createUser.Middlename = inputMiddlename;
            else
                throw new Exception("Middlename is null!");

            Console.Write("Date of birth: ");
            DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth);
            createUser.DateOfBirth = dateOfBirth;

            Console.Write("Login: ");
            string inputLogin = Console.ReadLine();
            string login = string.Empty;
            if (inputLogin != null && inputLogin.Length <= 20)
                createUser.Login = inputLogin;
            else
                throw new Exception("Login is null!");

            Console.Write("Password: ");
            string inputPassword = Console.ReadLine();
            string password = string.Empty;
            if (inputPassword != null && inputPassword.Length <= 20)
                createUser.Password = inputPassword;
            else
                throw new Exception("Password is null!");
            return createUser;
        }
        public static string LoginInput()
        {
            Console.Write("Login: ");
            string inputLogin = Console.ReadLine();
            string login = string.Empty;
            if (inputLogin != null && inputLogin.Length <= 20)
                login = inputLogin;
            else
                throw new Exception("Login is null!");
            return login;
        }
        public static string PasswordInput()
        {
            Console.Write("Password: ");
            string inputPassword = Console.ReadLine();
            string password = string.Empty;
            if (inputPassword != null && inputPassword.Length <= 20)
                password = inputPassword;
            else
                throw new Exception("Password is null!");
            return password;
        }
        public static string MessageInput()
        {
            Console.WriteLine("Введите сообщение!");
            string _inputMessege = Console.ReadLine();
            if (string.IsNullOrEmpty(_inputMessege))
                throw new Exception("Поля Сообщение пусто!");
            else
                return _inputMessege;
        }
        
        public static MessageDto CreateMessage(User sender, User recipient)
        {
            MessageDto messageDto = new MessageDto() { MessageText = MessageInput(), Sender = sender, Recipient = recipient };
            return messageDto;
        }
        public static User RecipientUser(string login)
        {
            return userService.GetUser(login);
        }
        public static User RecipientManager(string login)
        {
            return managerService.GetUser(login);
        }
        //принт в консол ошибки
        static public void PrintErrorMessage(string messege) => Console.WriteLine(messege);
    }
}