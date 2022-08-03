using Microsoft.AspNetCore.Authorization;

namespace clinic_assessment_redone.Middleware.Authorization.Requirements
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string permission { get; set; }

        public PermissionRequirement(string permission)
        {
            this.permission = permission;
        }
    }
}
