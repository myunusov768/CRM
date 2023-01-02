using CrmAppNew.Model;
using System;

namespace CrmAppNew.DTO
{
    public sealed class MessageDto
    {
        public string MessageText { get; set; } = string.Empty;
        public User Sender { get; set; } = new User();
        public User Recipient { get; set; } = new User();
    }
}
