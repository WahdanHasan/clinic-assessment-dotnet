using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using clinic_assessment_redone.Api.Dto.User;
using clinic_assessment_redone.Helpers.Misc;
using clinic_assessment_redone.Middleware.Authorization.Requirements;
using clinic_assessment_redone.Middleware.ErrorHandler.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace clinic_assessment_redone.Middleware.Authorization.Handler
{
    public class PermissionAuthHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Permission, int> _permissionRepository;
        private readonly IConfiguration _configuration;

        public PermissionAuthHandler(IHttpContextAccessor httpContextAccessor, IRepository<Permission, int> permissionRepository, IConfiguration configuration)
        {
            _permissionRepository = permissionRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

            string jwtEncoded = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (jwtEncoded == null)
            {
                throw new RestException(StatusCodes.Status401Unauthorized, Consts.NO_ACCESS_TOKEN);
            }

            jwtEncoded = jwtEncoded.Substring(Consts.TOKEN_PREFIX.Length);

            var token = new JwtSecurityToken(jwtEncoded);


            // Validate that the JWT is valid


            // TODO: Validate the JWT hasn't been tampered with



            // Get role claim values
            List<string> jwtRoleNames = new List<string>();
            DateTime? accessTokenExpiry = null;
            foreach (Claim claim in token.Claims)
            {
                if (ClaimTypes.Role.Equals(claim.Type))
                {
                    jwtRoleNames.Add(claim.Value);
                }
                else if ("exp".Equals(claim.Type))
                {
                    accessTokenExpiry = Util.UnixTimestampToUTC(Int32.Parse(claim.Value));
                }
            }

            //// Validate that the JWT hasn't expired
            //if (accessTokenExpiry.HasValue && (DateTime.UtcNow > accessTokenExpiry.Value))
            //{
            //    throw new RestException(StatusCodes.Status401Unauthorized, Constants.JWT_EXPIRED);
            //}


            // Get permissions from roles
            List<RolePermissionsDto> rolePermissionsList = ((IPermissionRepository)_permissionRepository).GetAllRolePermissions();

            // Get pending requirements from context
            var pendingRequirements = context.PendingRequirements.ToList();
            
            // Loop through pending requirements. If they are of type of PermissionRequirement, then check if the user has the appropriate permission to access the resource.
            // Allow access based on the result
            foreach (var pRequirement in pendingRequirements)
            {
                if (pRequirement.GetType() == typeof(PermissionRequirement))
                {
                    foreach(string jwtRole in jwtRoleNames)
                    {
                        if (Util.roleHasPermission(rolePermissionsList, ((PermissionRequirement)pRequirement).permission, jwtRole))
                        {
                            context.Succeed(requirement);
                            return;
                        }
                    }

                    break;
                }
            }


            context.Fail();
            return;
        }
    }
}
