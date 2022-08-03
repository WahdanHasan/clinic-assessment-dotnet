using clinic_assessment.data.Models.Db;
using clinic_assessment_redone.Api.Dto;
using clinic_assessment_redone.Api.Dto.User;
using clinic_assessment_redone.Helpers.Controllers.Interfaces;
using clinic_assessment_redone.Middleware.ErrorHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace clinic_assessment_redone.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserHelper _userHelper;

        public UserController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }


        [ProducesResponseType(typeof(RestException), StatusCodes.Status400BadRequest)]

        [HttpPost("/api/register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserReqDto registerUserReq)
        {
            var res = await _userHelper.RegisterUser(registerUserReq);

            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var res = await _userHelper.GetAllUsers();

            return Ok(res);
        }

    }
}
