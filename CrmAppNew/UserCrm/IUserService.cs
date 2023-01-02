using CrmAppNew.DTO;
using CrmAppNew.Model;
using System;

namespace CrmAppNew.UserCRM
{
    interface IUserService
    {
        void СreateUser(CreateUserDto createUserDto);
        User OpenProfile(string loginUser, string passwordUser);
        User GetUser(string userLogin);
        bool UserDataChange(CreateUserDto createUserDto, string loginUser, string passwordUser);
        void DeleteUser(string loginUser, string passwordUser);


    }
}
