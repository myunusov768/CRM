using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;

namespace CrmAppNew.MessageCrm
{
    public sealed class MessageService
    {
        public readonly List<Message> _messages;
        public readonly List<User> _users;
        public MessageService(List<Message> messages, List<User> users) { _messages = messages; _users = users; }
        
        public Result<bool> CreateMessage(MessageDto messageDto, Guid UserId, UserRoll recipienRole)
        {
            if (string.IsNullOrEmpty(messageDto.Text))
            {
                var result = new Result<bool>()
                {
                    Error = Error.TextIsNull,
                    IsSuccessfully = false,
                    Message = "Text Is Null",
                    Payload = false
                };
                return result;
            }
            else
            {
                _messages.Add(new Message() { 
                    MessageId = Guid.NewGuid(),
                    MessageText = messageDto.Text,
                    UserId = UserId,
                    RecipienRole = recipienRole,
                    Date = DateTime.Now });
                var result = new Result<bool>() { Message = "Ok", IsSuccessfully = true, Payload = true };
                return result;
            }
        }

        public Result<List<Message>> GetMessages(Guid userId, UserRoll recipientRole) 
        {
            var _list = new List<Message>();
            var _message = _messages.FirstOrDefault(x => (x.UserId.Equals(userId) && x.RecipienRole.Equals(recipientRole)));
            
            if (_message == null)
            {
                var result = new Result<List<Message>>() {  Error = Error.SheetIsEmpty, Message = "Sheet Is Empty", IsSuccessfully = false };
                return result;
            }
            else
            {
                foreach (var message in _messages)
                {
                    if (message.UserId.Equals(userId) && message.RecipienRole.Equals(recipientRole))
                        _list.Add(new Message()
                        {
                            MessageId = message.MessageId,
                            Date = message.Date,
                            UserId = message.UserId,
                            RecipienRole = message.RecipienRole,
                            MessageText = message.MessageText
                        });
                }
                var result = new Result<List<Message>>() { Message = "Ok", IsSuccessfully = true, Payload = _list };
                return result;
            }
        }
    }
}
