using clinic_assessment.data.Models.Db.Entity;

namespace clinic_assessment_redone.Api.Dto.User
{
    public class RolePermissionsDto
    {
        public int roleId { get; set; }
        public string roleName { get; set; }
        public List<Permission> permissions { get; set; }
    }
}
