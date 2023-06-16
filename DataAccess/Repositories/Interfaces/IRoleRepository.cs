using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        void DeleteRole(ApplicationDbContext dbContext, Role role);
        Role FindRoleById(ApplicationDbContext dbContext, int id);
        List<Role> GetRoles(ApplicationDbContext dbContext);
        void SaveRole(ApplicationDbContext dbContext, Role role);
        void UpdateRole(ApplicationDbContext dbContext, Role role);
    }
}
