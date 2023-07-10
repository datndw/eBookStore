using BusinessObject;
using BusinessObject.DTOs;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void DeleteUser(ApplicationDbContext dbContext, UserDTO user);
        UserDTO FindUserById(ApplicationDbContext dbContext, int id);
        List<UserDTO> GetUsers(ApplicationDbContext dbContext);
        void SaveUser(ApplicationDbContext dbContext, UserForm user);
        void UpdateUser(ApplicationDbContext dbContext, UserDTO user);
        Credential Login(ApplicationDbContext dbContext, string emailAddress, string password);
    }

}
