using clinic_assessment.data.Models.Db;
using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Response;
using clinic_assessment_redone.Api.Dto.User;
using Microsoft.AspNetCore.Mvc;

namespace clinic_assessment_redone.Helpers.Controllers.Interfaces
{
    public interface IUserHelper
    {
        Task<IEnumerable<ClinicUser>> GetAllUsers();

        Task<SuccessResponse> RegisterUser(RegisterUserReqDto req);
    }
}
