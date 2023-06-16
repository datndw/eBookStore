using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Daos
{
    public class UserDAO
    {
        private static UserDAO _instance = null;
        private static readonly object _instanceLock = new();

        public static UserDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserDAO();
                    }
                }
                return _instance;
            }
        }

        public static List<User> GetUsers(ApplicationDbContext dbContext)
        {
            List<User> users;
            try
            {
                users = dbContext.Users.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }

        public static User Login(ApplicationDbContext dbContext, string emailAddress, string password)
        {
            var user = new User();
            try
            {
                user = dbContext.Users.AsNoTracking().Include(e => e.Role).FirstOrDefault(e => e.EmailAddress.Equals(emailAddress) && e.Password.Equals(password));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public static User FindUserById(ApplicationDbContext dbContext, int id)
        {
            var user = new User();
            try
            {
                user = dbContext.Users.AsNoTracking().Include(e => e.Role).FirstOrDefault(e => e.Id == id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public static void SaveUser(ApplicationDbContext dbContext, User user)
        {
            try
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateUser(ApplicationDbContext dbContext, User user)
        {
            try
            {
                dbContext.Users.Update(user);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteUser(ApplicationDbContext dbContext, User user)
        {
            try
            {
                var c = dbContext.Users.SingleOrDefault(e => e.Id.Equals(user.Id));
                if (c != null)
                {
                    dbContext.Users.Remove(c);
                }
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
