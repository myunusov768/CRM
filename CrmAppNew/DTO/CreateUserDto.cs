using System;

namespace CrmAppNew.DTO
{
    public sealed class CreateUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Middlename { get; set; } = string.Empty;
        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value < DateTime.Now && value > DateTime.MinValue)
                    _dateOfBirth = value;
                else
                    throw new Exception("Wrong date!");
            }
        }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
