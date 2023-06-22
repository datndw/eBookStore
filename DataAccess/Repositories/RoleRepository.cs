using AutoMapper;
using BusinessObject;
using BusinessObject.DTOs;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IMapper _mapper;
        public RoleRepository(IMapper mapper)
        {
            _mapper = mapper;
        }
        public void DeleteRole(ApplicationDbContext dbContext, RoleDTO rawRole) {
            try
            {
                var c = dbContext.Roles.SingleOrDefault(e => e.Id.Equals(rawRole.Id));
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

        public RoleDTO FindRoleById(ApplicationDbContext dbContext, int id)
        {
            var role = new RoleDTO();
            try
            {
                var rawRole = dbContext.Roles.AsNoTracking().FirstOrDefault(e => e.Id == id);
                role = _mapper.Map<RoleDTO>(role);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return role;
        }

        public List<RoleDTO> GetRoles(ApplicationDbContext dbContext)
        {
            List<RoleDTO> roles;
            try
            {
                roles = dbContext.Roles.AsNoTracking().Select(role => _mapper.Map<RoleDTO>(role)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return roles;
        }

        public void SaveRole(ApplicationDbContext dbContext, RoleDTO rawRole) {
            try
            {
                Role role = dbContext.Roles.FirstOrDefault(u => u.Id == rawRole.Id);
                role = _mapper.Map(rawRole, role);
                dbContext.Roles.Add(role);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void UpdateRole(ApplicationDbContext dbContext, RoleDTO rawRole) {
            try
            {
                Role role = dbContext.Roles.FirstOrDefault(u => u.Id == rawRole.Id);
                role = _mapper.Map(rawRole, role);
                dbContext.Roles.Update(role);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
