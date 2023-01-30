using CrmAppNew.Abstracts;
using CrmAppNew.AdminCrm;
using CrmAppNew.Enums;

namespace CrmAppNew.Interfaces
{
    public static class AdminInterface
    {
        static public AdminService adminService = new AdminService(Program._usersList, Program._transactions, Program._employeeList);
        public static void Admin()
        {
            string command = Program.InputCommand();
            if(command.Equals("open"))
            {
                var user = Program.OpenProfile(adminService);
                int i = 0;
                while (i++ < 10)
                {
                    command = Program.InputCommand();
                    if (command.ToLower().Equals("get specific user"))
                        adminService.GetSpecificUser(Program.LoginInput());
                    else if (command.ToLower().Equals("get all users"))
                        adminService.GetAllUsers();
                    else if (command.ToLower().Equals("change"))
                        Program.UserDataChange(user.Id, adminService);
                    else if (command.ToLower().Equals("messanger"))
                        ChatInterface.CrmChat(user.UserRoll);
                    else if (command.ToLower().Equals("delete"))
                        Program.DeleteUser(user.Id, adminService);
                    else if (command.ToLower().Equals("create loan"))
                        RequestLoanUser(adminService);
                    else if (command.ToLower().Equals("check loan"))
                        ManagerInterface.LoanManagerCheck(adminService);
                    else if (command.ToLower().Equals("change status"))
                        ChangeStatusUser();
                    else if (command.ToLower().Equals("repayment loan"))
                        RepaymentLoan(adminService);
                    else if (command.ToLower().Equals("open card"))
                        Program.CreateCard(user.Id);
                    else if (command.ToLower().Equals("get cash"))
                        Program.GetCash(Program.AmountInput());
                    else if (command.ToLower().Equals("transfer"))
                        Program.DoTransfer(Program.AmountInput(), user.Id);
                    else if (command.ToLower().Equals("exit"))
                        return;
                    else
                        Console.WriteLine("Команда {command} некорректно");
                }
            }
            else if(command.Equals("registr"))
                Program.CreateUser(adminService);
            else
                Console.WriteLine("Команда {command} некорректно");
        }

        public static void ChangeStatusUser()
        {
            var loginInput = Program.LoginInput();
            var status = ChoiceStatus();
            var result = adminService.ChangeStatusUser(loginInput, status);
            if(result.IsSuccessfully)
                Console.WriteLine(result.Message);
            else
                Console.WriteLine(result.Message);
        }

        public static UserStatus ChoiceStatus()
        {
            Console.WriteLine("Vedite close or open");
            string inputStatus = Console.ReadLine();
            if (inputStatus.ToLower().Equals("close"))
                return UserStatus.Close;
            if (inputStatus.ToLower().Equals("open"))
                return UserStatus.Open;
            else
                throw new Exception("wrong command");
        }
        public static void RequestLoanUser(ILoanService service)
        {
            var result = adminService.GetSpecificUser(Program.LoginInput());
            if (result.IsSuccessfully)
            {
                Console.WriteLine(result.Message);
                LoanInterface.RequestLoan(result.Payload, service);
            }
            else
                throw new Exception(result.Message);
        }
        public static void RepaymentLoan(ILoanService service)
        {
            var result = adminService.GetSpecificUser(Program.LoginInput());
            if (result.IsSuccessfully)
            {
                Console.WriteLine(result.Message);
                LoanInterface.RepaymentLoan(result.Payload, service);
            }
            else
                throw new Exception(result.Message);
        }
    }
}
