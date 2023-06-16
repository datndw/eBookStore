using BusinessObject;
using DataAccess.Daos;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public void DeleteUser(ApplicationDbContext dbContext, User user) => UserDAO.DeleteUser(dbContext, user);

        public User FindUserById(ApplicationDbContext dbContext, int id) => UserDAO.FindUserById(dbContext, id);

        public List<User> GetUsers(ApplicationDbContext dbContext) => UserDAO.GetUsers(dbContext);

        public void SaveUser(ApplicationDbContext dbContext, User user) => UserDAO.SaveUser(dbContext, user);

        public void UpdateUser(ApplicationDbContext dbContext, User user) => UserDAO.UpdateUser(dbContext, user);
        public User Login(ApplicationDbContext dbContext, string emailAddress, string password) => UserDAO.Login(dbContext, emailAddress, password);
    }
}
