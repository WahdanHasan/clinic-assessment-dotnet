using clinic_assessment_redone.Api.Dto.Auth;
using clinic_assessment_redone.Helpers.Controllers.Interfaces;
using clinic_assessment_redone.Middleware.ErrorHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace clinic_assessment_redone.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthHelper _authHelper;

        public AuthController(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }


        [ProducesResponseType(typeof(RestException), StatusCodes.Status401Unauthorized)]


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDto loginReqDto)
        {
            var res = await _authHelper.AuthenticateUser(loginReqDto);

            return Ok(res);
        }

    }
}
