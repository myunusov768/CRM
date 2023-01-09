using CrmAppNew.Enums;


/*
 * Класс User только определяет сущность и нельзя наследовать и поставят печать seald
 */


namespace CrmAppNew.Model
{
    sealed public record class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Middlename { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRoll UserRoll { get; set; }
        public string Сomment { get; set; } = string.Empty;
        public ModeratorCheckType moderatorCheck { get; set; }
    }
}
