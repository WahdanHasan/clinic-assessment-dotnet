using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment_redone.Helpers.Dto;

namespace clinic_assessment_redone.Api.Dto.User
{
    public class RegisterUserReqDto
    {

        public UserDetails user { get; set; }

        public List<UserRoles>? userRoles { get; set; }

        public GetDoctorResDtoDto? employee { get; set; }

        public PatientDetails? patient { get; set; }

    }
}
