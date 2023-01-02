using CrmAppNew.Enums;

namespace CrmAppNew.Model
{
    public sealed class Message
    {
        public int MessageId { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public User Sender { get; set; } = new User();
        public User Recipient { get; set; } = new User();
        public DateTime Date { get; set; }
    }
}
