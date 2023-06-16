using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Daos
{
    public class RoleDAO
    {
        private static RoleDAO _instance = null;
        private static readonly object _instanceLock = new();

        public static RoleDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new RoleDAO();
                    }
                }
                return _instance;
            }
        }

        public static List<Role> GetRoles(ApplicationDbContext dbContext)
        {
            List<Role> roles;
            try
            {
                roles = dbContext.Roles.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return roles;
        }

        public static Role FindRoleById(ApplicationDbContext dbContext, int id)
        {
            var role = new Role();
            try
            {
                role = dbContext.Roles.AsNoTracking().FirstOrDefault(e => e.Id == id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return role;
        }

        public static void SaveRole(ApplicationDbContext dbContext, Role role)
        {
            try
            {
                dbContext.Roles.Add(role);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateRole(ApplicationDbContext dbContext, Role role)
        {
            try
            {
                dbContext.Entry(role).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteRole(ApplicationDbContext dbContext, Role role)
        {
            try
            {
                var c = dbContext.Roles.SingleOrDefault(e => e.Id.Equals(role.Id));
                if (c != null)
                {
                    dbContext.Roles.Remove(c);
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
