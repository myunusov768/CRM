using CrmAppNew.DTO;

namespace CrmAppNew.Model
{
    public abstract class AbstractUser
    {
        public abstract Result<bool> СreateUser(CreateUserDto createUserDto);
        public abstract Result<User> OpenProfile(string loginUser, string passwordUser);
        public abstract Result<bool> UserDataChange(CreateUserDto createUserDto, Guid userId);
        public abstract Result<bool> DeleteUser(Guid userId);
    }
}
