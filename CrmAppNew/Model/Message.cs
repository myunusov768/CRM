using CrmAppNew.Enums;

namespace CrmAppNew.Model
{
    public sealed class Message
    {
        public Guid MessageId { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public UserRoll RecipienRole { get; set; }
        public DateTime Date { get; set; }
    }
}
