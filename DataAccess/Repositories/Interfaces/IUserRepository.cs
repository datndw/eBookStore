using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void DeleteUser(ApplicationDbContext dbContext, User user);
        User FindUserById(ApplicationDbContext dbContext, int id);
        List<User> GetUsers(ApplicationDbContext dbContext);
        void SaveUser(ApplicationDbContext dbContext, User user);
        void UpdateUser(ApplicationDbContext dbContext, User user);
        User Login(ApplicationDbContext dbContext, string emailAddress, string password);
    }

}
