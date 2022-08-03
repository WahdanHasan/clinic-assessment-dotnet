using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment_redone.Api.Dto.Auth;
using clinic_assessment_redone.Api.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Repo.Interfaces
{
    public interface IPermissionRepository : IRepository<Permission, int>
    {
        Task<List<UserPermissionDto>> GetUserRolePermissions(int userId);

        List<RolePermissionsDto> GetAllRolePermissions();
    }
}
