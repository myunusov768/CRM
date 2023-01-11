using CrmAppNew.DTO;
using CrmAppNew.LoanCrm;
using CrmAppNew.Model;
using CrmAppNew.UserCrm;
using CrmAppNew.Interfaces;
using System;


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
                            RequestLoan(user);
                        else if (command.ToLower().Equals("loans"))
                            AllLoans(user);
                        else if (command.ToLower().Equals("repayment loan"))
                            RepaymentLoan(user);
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

        public static void RequestLoan(User user)
        {
            Console.WriteLine("Введите сумма кредита!");
            int amount = int.Parse(Console.ReadLine());
            var result = Program.loanService.CreateLoan(user.Id, amount);
            Console.WriteLine(result.Message);
        }
        public static void AllLoans(User user)
        {
            var loans = Program.loanService.GetAllLoansUser(user.Id);
            if (!loans.IsSuccessfully)
                Console.WriteLine(loans.Message);
            else
                Console.WriteLine(loans.Payload);
        }
        public static void RepaymentLoan(User user)
        {
            AllLoans(user);
            Console.WriteLine("Введите Id транша!");
            int _id = int.Parse(Console.ReadLine());
            var loan = Program.loanService.GetSpecificLoan(_id);
            if (loan.IsSuccessfully)
            {
                Console.Write($"Сумма: {loan.Payload.LoanAmount}");

                var result = Program.loanService.RepaymentLoan(_id, Program.AmountInput());
                Console.WriteLine(result.Message);
            }
            else
                Console.WriteLine("Не правельный транш!");
            
        }
    }
}
