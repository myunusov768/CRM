using CrmAppNew.Model;
using CrmAppNew.UserCrm;



namespace CrmAppNew.Interfaces
{

    public static class UserInterface
    {
        static public UserService userService = new UserService(Program._usersList);

        public static void User()
        {
            string command = Program.InputCommand();
            if (command.ToLower().Equals("open"))
            {
                var user = Program.OpenProfile(userService);
                if (user == null)
                    Console.WriteLine(user);
                else
                {
                    int i = 0;
                    while (i++ < 100)
                    {
                        command = Program.InputCommand();
                        if (command.ToLower().Equals("seeprofile"))
                            Program.SeeProfile(user);
                        else if (command.ToLower().Equals("change"))
                            Program.UserDataChange(user.Id, userService);
                        else if (command.ToLower().Equals("create loan"))
                            LoanInterface.RequestLoan(user, Program.loanService);
                        else if (command.ToLower().Equals("loans"))
                            LoanInterface.AllLoans(user, Program.loanService);
                        else if (command.ToLower().Equals("repayment loan"))
                            LoanInterface.RepaymentLoan(user, Program.loanService);
                        else if (command.ToLower().Equals("messanger"))
                            ChatInterface.UserChat(user);
                        else if (command.ToLower().Equals("delete"))
                            Program.DeleteUser(user.Id, userService);
                        else if (command.ToLower().Equals("exit"))
                            return;
                        else
                            throw new Exception("Команда {command} некорректно");
                    }
                }
            }
            else if (command.ToLower().Equals("registr"))
                Program.CreateUser(userService);
            else
                throw new Exception("Команда {command} некорректно");
        }
    }
}
