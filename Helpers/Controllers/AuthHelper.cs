using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using clinic_assessment_redone.Api.Dto.Auth;
using clinic_assessment_redone.Helpers.Controllers.Interfaces;
using clinic_assessment_redone.Helpers.Misc;
using clinic_assessment_redone.Middleware.ErrorHandler.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace clinic_assessment_redone.Helpers.Controllers
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IRepository<ClinicUser, int> _userRepository;
        private readonly IRepository<Permission, int> _permissionRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly IConfiguration _configuration;
        public AuthHelper(IRepository<ClinicUser, int> userRepository, IRepository<Permission, int> permissionRepository, IRepository<Role, int> roleRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
        }

        public async Task<LoginResDto> AuthenticateUser(LoginReqDto loginReqDto)
        {
            /* Get user from DB */
            ClinicUser clinicUser = await ((IClinicUserRepository)_userRepository).GetByEmail(loginReqDto.userName);

            /* Validate that the user exists */
            if (clinicUser == null)
            {
                throw new RestException(StatusCodes.Status401Unauthorized, Constants.INVALID_CREDENTIALS);
            }

            /* Verify password */
            bool isPasswordMatch = Util.verifyPassword(loginReqDto.password, clinicUser.Password);

            if (!isPasswordMatch)
            {
                throw new RestException(StatusCodes.Status401Unauthorized, Constants.INVALID_CREDENTIALS);
            }

            /* Generate JWT */
            List<Role> userRoles = await ((IRoleRepository)_roleRepository).GetUserRoles(clinicUser.Id);

            // Set token claims
            List<Claim> userClaims = new List<Claim>();

            // Get user roles and set role claims
            foreach (Role role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            DateTime accessTokenExpiry = DateTime.Now.AddMinutes(Constants.JWT_ACCESS_EXPIRY);
            LoginResDto loginResDto = new LoginResDto
            {
                accessToken = Util.CreateToken(
                    clinicUser.Email, 
                    _configuration,
                    userClaims,
                    accessTokenExpiry
                    ),
                refreshToken = Util.CreateToken(
                    clinicUser.Email,
                     _configuration,
                    userClaims,
                    accessTokenExpiry.AddMinutes(Constants.JWT_REFRESH_EXPIRY)
                    )
            };

            return loginResDto;
        }


    }
}
