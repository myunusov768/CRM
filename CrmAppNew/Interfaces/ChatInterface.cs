using CrmAppNew.Enums;
using CrmAppNew.Model;
using System;

namespace CrmAppNew.Interfaces
{
    public static class ChatInterface
    {
        public static void UserChat(User user)
        {
            int i =0;
            while (100 > ++i)
            {
                Console.WriteLine("selec role");
                string _inputRole = Console.ReadLine();
                if (_inputRole is null) { return; }
                else if (_inputRole.ToLower().Equals("admin"))
                {
                    OpenChat(user.Id, UserRoll.Admin);
                }
                else if (_inputRole.ToLower().Equals("manager"))
                {
                    OpenChat(user.Id, UserRoll.Manager);
                }
                else if (_inputRole.ToLower().Equals("exit"))
                    return;
                else
                    Console.WriteLine("Ooooops:(");
            }
        }
        public static void OpenChat(Guid userId, UserRoll recipienRole)
        {
            PrintMessages(userId, recipienRole);
            int i = 0;
            while (100 > ++i)
            {
                string сommand = Program.InputCommand();
                if (сommand is null) { return; }
                else if (сommand.ToLower().Equals("send"))
                {
                    var messageDto = Program.CreateMessage();
                    var result = Program.messageService.CreateMessage(messageDto, userId, recipienRole);
                    if (result.IsSuccessfully)
                        Console.WriteLine(result.Message);
                    else
                        Console.WriteLine(result.Message);
                }
                else if (сommand.ToLower().Equals("exit"))
                    return;
                else
                    Console.WriteLine("Ooooops:(");
            }
        }
        public static void PrintMessages(Guid userId, UserRoll recipientRole)
        {
            var _listMessages = Program.messageService.GetMessages(userId, recipientRole);
            if (_listMessages.IsSuccessfully)
            {
                foreach (var item in _listMessages.Payload)
                    Console.WriteLine($"{item.Date.ToShortDateString()} {item.MessageText}");
            }
            Console.WriteLine($"{_listMessages.Message}");
        }
        public static void CrmChat(UserRoll CrmRole)
        {
            int i = 0;
            while (100 > ++i)
            {
                Console.WriteLine("selec user(Login)");
                var SelectUser = UserInterface.userService.GetUserWithLogin(Program.LoginInput());
                if (SelectUser.IsSuccessfully)
                {
                    string _command = Console.ReadLine();
                    if (_command is null) { Console.WriteLine("error"); }
                    else if (_command.ToLower().Equals("send"))
                    {
                        var messageDto = Program.CreateMessage();
                        var result = Program.messageService.CreateMessage(messageDto, SelectUser.Payload.Id, CrmRole);
                        if (result.IsSuccessfully)
                            Console.WriteLine(result.Message);
                        Console.WriteLine(result.Message);
                    }
                    else if (_command.ToLower().Equals("exit"))
                        return;
                    else
                        Console.WriteLine("Ooooops:(");
                }
                Console.WriteLine(SelectUser.Message);
            }
        }
    }
}
