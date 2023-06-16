using BusinessObject;
using DataAccess.Daos;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public void DeleteRole(ApplicationDbContext dbContext, Role role) => RoleDAO.DeleteRole(dbContext, role);

        public Role FindRoleById(ApplicationDbContext dbContext, int id) => RoleDAO.FindRoleById(dbContext, id);

        public List<Role> GetRoles(ApplicationDbContext dbContext) => RoleDAO.GetRoles(dbContext);

        public void SaveRole(ApplicationDbContext dbContext, Role role) => RoleDAO.SaveRole(dbContext, role);

        public void UpdateRole(ApplicationDbContext dbContext, Role role) => RoleDAO.UpdateRole(dbContext, role);
    }
}
