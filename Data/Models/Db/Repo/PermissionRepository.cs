using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using clinic_assessment_redone.Api.Dto.Auth;
using clinic_assessment_redone.Api.Dto.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace clinic_assessment.data.Models.Db.Repo
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DataContext _dataContext;
        public PermissionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Permission>> GetAll()
        {
            throw new NotImplementedException();
        }


        public Task<Permission> GetById(int id)
        {
            throw new NotImplementedException();
        }
        public List<RolePermissionsDto> GetAllRolePermissions()
        {

            List<RolePermissionsDto> rolePermissions = _dataContext.Role
                .Select(rpd => new RolePermissionsDto
                {
                    roleId = rpd.Id,
                    roleName = rpd.Name,
                    permissions = _dataContext.Permission
                    .Where(p => p.PermissionRoles.Where(pr => pr.Role.Id == rpd.Id).Count() > 0)
                    .ToList()
                }).ToList();

            return rolePermissions;
        }

        public async Task<List<UserPermissionDto>> GetUserRolePermissions(int userId)
        {     

            var userPermissions = await _dataContext.Permission
                .Join(_dataContext.RolePermission,
                permission => permission.Id,
                rolePermission => rolePermission.Permission.Id,
                (permission, rolePermission) => new { permission, rolePermission }
                )
                .Join(_dataContext.UserRole,
                combinedRolePermission => combinedRolePermission.rolePermission.Role.Id,
                userRole => userRole.Role.Id,
                (combinedRolePermission, userRole) => new UserPermissionDto
                {
                    userId = userRole.User.Id,
                    permission = combinedRolePermission.permission.Name
                }
                )
                .Where(fullPermission => fullPermission.userId == userId)
                .ToListAsync();


            return userPermissions;
        }

        public Task<Permission> Insert(Permission entity)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
