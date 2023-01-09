using CrmAppNew.AdminCrm;
using CrmAppNew.DTO;
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
                    Console.WriteLine("Iput comand:> ");
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
    }
}
