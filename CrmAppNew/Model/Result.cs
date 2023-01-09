
using CrmAppNew.Enums;

namespace CrmAppNew.Model
{
    public sealed class Result<T>
    {
        public Error Error { get; set; }
        public bool IsSuccessfully { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Payload { get; set; }
    }
}
