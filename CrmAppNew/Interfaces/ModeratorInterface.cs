using CrmAppNew.AdminCrm;
using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;
using CrmAppNew.ModeratorCrm;

namespace CrmAppNew.Interfaces
{
    public static class ModeratorInterface
    {
        static public ModeratorService moderatorService = new ModeratorService(Program._usersList);

        public static void Moderator()
        {
            string command = Program.InputCommand();
            if (command.ToLower().Equals("open"))
            {
                var user = Program.OpenProfile(moderatorService);
                int i = 0;
                while (i++ < 100)
                {
                    command = Program.InputCommand();

                    if (command.ToLower().Equals("start checking"))
                        ModeratorProseccChekc();
                    else if (command.ToLower().Equals("delete"))
                        Program.DeleteUser(user.Id, moderatorService);
                    else if (command.ToLower().Equals("change"))
                        Program.UserDataChange(user.Id, moderatorService);
                    else if (command.ToLower().Equals("exit"))
                        return;
                    else
                        throw new Exception("Команда {command} некорректно");
                }
            }
            else if (command.ToLower().Equals("registr"))
                Program.CreateUser(moderatorService);
            else
                throw new Exception("Команда {command} некорректно");
        }
        public static void ModeratorProseccChekc()
        {
            foreach (var item in Program._usersList)
            {
                if (item.moderatorCheck.Equals(ModeratorCheckType.Pending))
                {
                    Console.WriteLine($"FLM: {item.FirstName} {item.LastName} {item.Middlename}" +
                        $"\nDate of birth: {item.DateOfBirth}, Age: {item.Age}\n");
                    string command = Program.InputCommand();
                    Console.WriteLine("Iput comment please!");
                    string comment = Console.ReadLine();
                    if (command.ToLower().Equals("+"))
                        if (comment != null)
                            moderatorService.ModeratorServiceCheck(item.Id, ModeratorCheckType.Accept, comment);
                        else
                            throw new Exception("Команда {command} некорректно");
                    else if (command.ToLower().Equals("-"))
                        if (comment != null)
                            moderatorService.ModeratorServiceCheck(item.Id, ModeratorCheckType.NotAccept, comment);
                        else
                            throw new Exception("Команда {command} некорректно");
                    else if (command.ToLower().Equals("exit"))
                        break;
                    else
                        throw new Exception("Команда {command} некорректно");
                }
            }
        }
    }
}
