using CrmAppNew.Abstracts;
using CrmAppNew.Enums;
using CrmAppNew.ManagerCrm;

namespace CrmAppNew.Interfaces
{
    static class ManagerInterface
    {
        static public ManagerService managerService = new ManagerService(Program._usersList, Program._transactions);
        public static void Manager()
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
                        LoanManagerCheck(managerService);
                    else if (command.ToLower().Equals("statistic"))
                        CheckingStatistic();
                    else if (command.ToLower().Equals("messanger"))
                        ChatInterface.CrmChat(user.UserRoll);
                    else if (command.ToLower().Equals("delete"))
                        Program.DeleteUser(user.Id, managerService);
                    else if (command.ToLower().Equals("change"))
                        Program.UserDataChange(user.Id, managerService);
                    else if (command.ToLower().Equals("open card"))
                        Program.CreateCard(user.Id);
                    else if (command.ToLower().Equals("get cash"))
                        Program.GetCash(Program.AmountInput());
                    else if (command.ToLower().Equals("transfer"))
                        Program.DoTransfer(Program.AmountInput(), user.Id);
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
        public static void LoanManagerCheck(IManagerService service)
        {
            foreach (var item in Program._transactions)
            {
                if (item.LoanType.Equals(LoanType.Pending))
                {
                    Console.WriteLine($"tranche: {item.Tranche}" +
                        $"\nDate of loan: {item.DateLoan}, Amount: {item.LoanAmount}, Loan balance {item.LoanBalance}\n");
                    
                    string command = Program.InputCommand();
                    Console.WriteLine("Iput comment please!");
                    string comment = Console.ReadLine();
                    if(string.IsNullOrEmpty(comment))
                        throw new Exception("Команда {command} некорректно");
                    else if (command.ToLower().Equals("+"))
                        service.ManagerServiceCheck(item.Id, LoanType.Accept, comment);
                    else if (command.ToLower().Equals("-"))
                        service.ManagerServiceCheck(item.Id, LoanType.NotAccept, comment);
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
