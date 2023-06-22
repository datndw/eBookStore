using BusinessObject;
using BusinessObject.DTOs;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        void DeleteRole(ApplicationDbContext dbContext, RoleDTO role);
        RoleDTO FindRoleById(ApplicationDbContext dbContext, int id);
        List<RoleDTO> GetRoles(ApplicationDbContext dbContext);
        void SaveRole(ApplicationDbContext dbContext, RoleDTO role);
        void UpdateRole(ApplicationDbContext dbContext, RoleDTO role);
    }
}
