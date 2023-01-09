using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.ManagerCrm;
using CrmAppNew.Model;
using CrmAppNew.ModeratorCrm;
using System;
using System.Transactions;

namespace CrmAppNew.Interfaces
{
    static class ManagerInterface
    {
        static public ManagerService managerService = new ManagerService(Program._usersList, Program._transactions);
        static void Manager()
        {
            string command = Program.InputCommand();
            if (command.ToLower().Equals("open"))
            {
                var user = Program.OpenProfile(managerService);
                int i = 0;
                while (i++ < 100)
                {
                    command = Program.InputCommand();

                    if (command.ToLower().Equals("start checking"))
                        LoanManagerCheck();
                    else if (command.ToLower().Equals("statistic"))
                        CheckingStatistic();
                    else if (command.ToLower().Equals("messanger"))
                        ChatInterface.CrmChat(user.UserRoll);
                    else if (command.ToLower().Equals("delete"))
                        Program.DeleteUser(user.Id, managerService);
                    else if (command.ToLower().Equals("change"))
                        Program.UserDataChange(user.Id, managerService);
                    else if (command.ToLower().Equals("exit"))
                        return;
                    else
                        throw new Exception("Команда {command} некорректно");
                }
            }
            else if (command.ToLower().Equals("registr"))
                Program.CreateUser(managerService);
            else
                throw new Exception("Команда {command} некорректно");
        }
        public static void LoanManagerCheck()
        {
            foreach (var item in Program._transactions)
            {
                if (item.LoanType.Equals(LoanType.Pending))
                {
                    Console.WriteLine($"FLM: {item.UserId}" +
                        $"\nDate of loan: {item.DateLoan}, Amount: {item.LoanAmount}, Loan balance {item.LoanBalance}\n");
                    
                    string command = Program.InputCommand();
                    Console.WriteLine("Iput comment please!");
                    string comment = Console.ReadLine();
                    if(string.IsNullOrEmpty(comment))
                        throw new Exception("Команда {command} некорректно");
                    else if (command.ToLower().Equals("+"))
                        managerService.ManagerServiceCheck(item.Id, LoanType.Accept, comment);
                    else if (command.ToLower().Equals("-"))
                        managerService.ManagerServiceCheck(item.Id, LoanType.NotAccept, comment);
                    else if (command.ToLower().Equals("exit"))
                        break;
                    else
                        throw new Exception("Команда {command} некорректно");
                }
            }
        }
        public static void CheckingStatistic()
        {
            StatisticService statisticService = new StatisticService(Program._usersList);
            statisticService.CalculationStatistic();
        }
    }
}
