using CrmAppNew.UserCrm;
using CrmAppNew.DTO;
using CrmAppNew.Model;
using CrmAppNew.AdminCrm;
using CrmAppNew.ModeratorCrm;
using CrmAppNew.ManagerCrm;
using CrmAppNew.LoanCrm;
using CrmAppNew.MessageCrm;
using CrmAppNew.Interfaces;
using CrmAppNew.UserCRM;

namespace CrmAppNew
{
     class Program
    {
        static public List<Loan> _transactions = new List<Loan>();
        static public List<Message> _message = new List<Message>();
        static public List<User> _usersList = new List<User>();
        
        static public LoanServise loanService = new LoanServise(_transactions);
        static public MessageService messageService = new MessageService(_message, _usersList);
        static void Main(string[] args)
        {
            _usersList.Add(new User() { Login = "user", Password = "786", UserRoll = Enums.UserRoll.User, moderatorCheck = Enums.ModeratorCheckType.Accept });
            _usersList.Add(new User() { Login = "admin", Password = "786", UserRoll = Enums.UserRoll.Admin });
            _usersList.Add(new User() { Login = "manag", Password = "786", UserRoll = Enums.UserRoll.Manager });
            _usersList.Add(new User() { Login = "moder", Password = "786", UserRoll = Enums.UserRoll.Moderator });



            int i = 0;
            while (i++ < 100)
            {
                try
                {
                    Console.Write("Enter your role :> ");
                    string command = Console.ReadLine();
                    if (command.ToLower().Equals("user"))
                        UserInterface.User();
                    else if (command.ToLower().Equals("admin"))
                        AdminInterface.Admin();
                    else if (command.ToLower().Equals("moderator"))
                        ModeratorInterface.Moderator();
                    else if (command.ToLower().Equals("manager"))
                        AdminInterface.Admin();
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
            Console.ReadLine();
        }
        

        
        //---------------------------------------------------------------------------------------------

        public static void SeeProfile(User user)
        {
            Console.WriteLine($"FLM: {user.FirstName} {user.LastName} {user.Middlename}\nDate of birth: {user.DateOfBirth}, Age: {user.Age}\n");
        }
        public static string InputCommand()
        {
            Console.Write("Iput comand:> ");
            string command = Console.ReadLine();
            if (command == null)
                throw new ArgumentNullException();
            else
                return command;
        }
        public static Guid InputId()
        {
            Console.Write("Id: ");
            string inputId = Console.ReadLine();
            Guid Id;
            bool ss = Guid.TryParse(inputId, out Id);
            if (ss)
                return Id;
            else
                throw new Exception("Login is null!");
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
        public static MessageDto CreateMessage()
        {
            MessageDto messageDto = new MessageDto() { Text = MessageInput() };
            return messageDto;
        }
        public static User RecipientUser(Guid loginId)
        {
            var result = UserInterface.userService.GetUser(loginId);
            return result.Payload;
        }
        public static User RecipientManager(Guid loginId)
        {
            return UserInterface.userService.GetUser(loginId).Payload;
        }
        public static void CreateUser(AbstractUser user)
        {

            var userInput = СreateUserInput();
            user.СreateUser(new CreateUserDto()
            {
                FirstName = userInput.FirstName,
                LastName = userInput.LastName,
                Middlename = userInput.Middlename,
                DateOfBirth = userInput.DateOfBirth,
                Login = userInput.Login,
                Password = userInput.Password,
            });
        }
        public static void UserDataChange(Guid userId, AbstractUser user)
        {
            var userDto = СreateUserInput();
            var ff = user.UserDataChange(new CreateUserDto
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Middlename = userDto.Middlename,
                DateOfBirth = userDto.DateOfBirth,
                Login = userDto.Login,
                Password = userDto.Password,
            }, userId);
            if (ff.IsSuccessfully)
                Console.WriteLine("Данные успешно изменено!");
            else
                Console.WriteLine("Упс:(");
        }
        public static User OpenProfile(AbstractUser user)
        {
            var result = user.OpenProfile(LoginInput(), PasswordInput());
            if (result.IsSuccessfully)
            {
                Console.WriteLine($"Welcome {result.Payload.FirstName} {result.Payload.LastName} {result.Payload.Middlename}!!!\n");
                return result.Payload;
            }
            else
                throw new Exception(result.Message);
        }
        public static bool DeleteUser(Guid userId,AbstractUser user)
        {
            var result = user.DeleteUser(userId);
            if (result.IsSuccessfully)
            {
                Console.WriteLine(result.Message);
                return result.Payload;
            }
            else
                throw new Exception(result.Message);
        }
    }
}