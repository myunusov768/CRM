using CrmAppNew.Enums;
using System;

namespace CrmAppNew.MessageCrm
{
    public sealed class MessageResult<T>
    {
        public Error Error { get; set; }
        public MessageStatus IsSuccessfully { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Payload { get; set; }
    }
}
