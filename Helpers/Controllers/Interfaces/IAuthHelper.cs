using clinic_assessment_redone.Api.Dto.Auth;

namespace clinic_assessment_redone.Helpers.Controllers.Interfaces
{
    public interface IAuthHelper
    {
        Task<LoginResDto> AuthenticateUser(LoginReqDto loginReqDto);
    }
}
