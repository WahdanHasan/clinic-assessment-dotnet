using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment_redone.Api.Dto.Doctor;
using clinic_assessment_redone.Helpers.Dto;

namespace clinic_assessment_redone.Helpers.Mapper
{
    public class DoctorMapper
    {
        public static DoctorResDto doctorEntityToGetDoctorResDtoDto(Doctor doctor)
        {
            return new DoctorResDto
            {
                id = doctor.Id,
                firstName = doctor.FirstName,
                lastName = doctor.LastName,
                gender = doctor.Gender,
                nationality = doctor.Nationality,
                phoneExtension = doctor.PhoneExt,
                officeRoomNumber = doctor.OfficeRoomNumber,
                specialty = doctor.specialty
            };
        }
    }
}
