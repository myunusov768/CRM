using CrmAppNew.AdminCrm;
using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;
using CrmAppNew.UserCrm;

namespace CrmAppNew.Interfaces
{
    public static class AdminInterface
    {
        static public AdminService adminService = new AdminService(Program._usersList);
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
                        CreateLoan();
                    else if (command.ToLower().Equals("check loan"))
                        CheckLoanAdmin();
                    else if (command.ToLower().Equals("change status"))
                        ChangeStatusUser();
                    else if (command.ToLower().Equals("repayment loan"))
                        RepaymentLoan();
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

        public static void CreateLoan()
        {
            var result = adminService.GetSpecificUser(Program.LoginInput());
            if (!result.IsSuccessfully)
            {
                Console.WriteLine(result.Message);
                return;
            }
            UserInterface.RequestLoan(result.Payload);
        }
        public static void CheckLoanAdmin()
        {
            ManagerInterface.LoanManagerCheck();
        }
        public static void RepaymentLoan()
        {
            var result = adminService.GetSpecificUser(Program.LoginInput());
            if (!result.IsSuccessfully)
            {
                Console.WriteLine(result.Message);
                return;
            }
            UserInterface.RepaymentLoan(result.Payload);
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
    }
}
