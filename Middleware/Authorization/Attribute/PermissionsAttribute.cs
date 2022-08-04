using clinic_assessment_redone.Helpers.Misc;
using Microsoft.AspNetCore.Authorization;

namespace clinic_assessment_redone.Middleware.Authorization.Attribute
{
    public class PermissionsAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = Consts.CLAIM_TYPE_PERMISSION;

        public PermissionsAttribute(string permissionName) => PermissionName = permissionName;

        public string PermissionName
        {
            get
            {
                return PermissionName;
            }
            set
            {
                Policy = $"{value.ToString()}";
            }
        }
    }
}
