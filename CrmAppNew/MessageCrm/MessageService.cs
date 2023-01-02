using CrmAppNew.DTO;
using CrmAppNew.Enums;
using CrmAppNew.Model;
using System;


namespace CrmAppNew.MessageCrm
{
    public sealed class MessageService
    {
        public readonly List<Message> _messages;
        public MessageService(List<Message> messages) { _messages = messages; }
        private int _id;
        private int CreateID()=>_id++;
        
        public MessageResult<string> CreateMessage(MessageDto messageDto)
        {
            if (string.IsNullOrEmpty(messageDto.MessageText))
            {
                var result = new MessageResult<string> { 
                    Error = Error.TextIsNull,
                    IsSuccessfully = MessageStatus.Failed,
                    Message = string.Empty, 
                    Payload = string.Empty};
                return result;
            }
            else
            {
                _messages.Add(new Message() { 
                    MessageId = CreateID(),
                    MessageText = messageDto.MessageText, 
                    Sender = messageDto.Sender,
                    Recipient= messageDto.Recipient,
                    Date = DateTime.Now });
                var result = new MessageResult<string> { Message = messageDto.MessageText, IsSuccessfully = MessageStatus.Sent };
                return result;
            }
        }
        public List<Message> GetMessages(User sender, User recipient) 
        {
            var result = new List<Message>();
            var _message = _messages.FirstOrDefault(x => (x.Sender.Login.Equals(sender.Login) && x.Recipient.Equals(recipient.Login)) 
            || (x.Sender.Login.Equals(sender.Login) && x.Recipient.Equals(recipient.Login)));
            

            if (sender == null)
                throw new Exception("User's empty!");
            else if(_message == null)
                throw new Exception("Message's not found!");
            else
            {
                foreach (var message in _messages)
                {
                    if (message.Sender.Equals(_message.Sender) && message.Recipient.Equals(_message.Recipient))
                    result.Add(new Message()
                    {
                        Date = message.Date,
                        Sender = message.Sender,
                        Recipient = message.Recipient,
                        MessageId = message.MessageId,
                        MessageText = message.MessageText
                    });
                }
                return result;
            }
        }
        
    }
}
