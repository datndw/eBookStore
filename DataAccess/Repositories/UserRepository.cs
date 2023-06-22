using AutoMapper;
using BusinessObject;
using BusinessObject.DTOs;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        public UserRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void DeleteUser(ApplicationDbContext dbContext, UserDTO rawUser) {
            try
            {
                var u = dbContext.Users.SingleOrDefault(e => e.Id.Equals(rawUser.Id));
                if (u != null)
                {
                    dbContext.Users.Remove(u);
                }
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public UserDTO FindUserById(ApplicationDbContext dbContext, int id)
        {
            var user = new UserDTO();
            try
            {
                var rawUser = dbContext.Users.AsNoTracking().Include(e => e.Publisher).Include(e => e.Role).FirstOrDefault(e => e.Id == id);
                user = _mapper.Map<UserDTO>(rawUser);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public List<UserDTO> GetUsers(ApplicationDbContext dbContext)
        {
            List<UserDTO> users;
            try
            {
                var rawUsers = dbContext.Users.AsNoTracking().Include(p => p.Publisher).Include(r => r.Role).ToList();
                users = _mapper.Map<List<UserDTO>>(rawUsers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }

        public void SaveUser(ApplicationDbContext dbContext, UserForm rawUser) {
            try
            {
                User user = new User();
                user = _mapper.Map<User>(rawUser);
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateUser(ApplicationDbContext dbContext, UserDTO rawUser) {
            try
            {
                User user = dbContext.Users.Include(p => p.Publisher).Include(r => r.Role).FirstOrDefault(u => u.Id == rawUser.Id);
                user = _mapper.Map<User>(rawUser);
                user.Password = "123";
                dbContext.Users.Update(user);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Credential Login(ApplicationDbContext dbContext, string emailAddress, string password)
        {
            var user = new Credential();
            try
            {
                var rawUser = dbContext.Users.AsNoTracking().Include(e => e.Role).FirstOrDefault(e => e.EmailAddress.Equals(emailAddress) && e.Password.Equals(password));
                user = _mapper.Map<Credential>(rawUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
    }
}
