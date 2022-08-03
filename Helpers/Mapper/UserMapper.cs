using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment_redone.Api.Dto.User;
using clinic_assessment_redone.Helpers.Misc;

namespace clinic_assessment_redone.Helpers.Mapper
{
    public class UserMapper
    {
        public static Patient RegisterUserReqDtoToPatientEntity(RegisterUserReqDto dto)
        {

            return new Patient
            {
                Email = dto.user.email,
                Password = dto.user.password,
                FirstName = dto.user.firstName,
                LastName = dto.user.lastName,
                DateOfBirth = Util.stringDateToDate(dto.user.dateOfBirth),
                Nationality = dto.user.nationality,
                Gender = dto.user.gender,
                PhoneCountryCode = dto.user.phoneCountryCode,
                PhoneNumber = dto.user.phoneNumber,
                UserRoles = dto.userRoles,
                bloodType = dto.patient.bloodType,
            };
        }

        public static Doctor RegisterUserReqDtoToDoctorEntity(RegisterUserReqDto dto)
        {
            List<TimeOnly> workStartTimes = new List<TimeOnly>();
            List<TimeOnly> workEndTimes = new List<TimeOnly>();

            for(int i = 0 ; i < dto.employee.workStartTimes.Count ; i++)
            {
                workStartTimes.Add(Util.stringTimeToTime(dto.employee.workStartTimes[i]));
                workEndTimes.Add(Util.stringTimeToTime(dto.employee.workEndTimes[i]));
            }

            return new Doctor
            {
                Email = dto.user.email,
                Password = dto.user.password,
                FirstName = dto.user.firstName,
                LastName = dto.user.lastName,
                DateOfBirth = Util.stringDateToDate(dto.user.dateOfBirth),
                Nationality = dto.user.nationality,
                Gender = dto.user.gender,
                PhoneCountryCode = dto.user.phoneCountryCode,
                PhoneNumber = dto.user.phoneNumber,
                UserRoles = dto.userRoles,
                PhoneExt = dto.employee.phoneExt,
                OfficeRoomNumber = dto.employee.officeRoomNo,
                WorkStartTimes = workStartTimes,
                WorkEndTimes = workEndTimes,
                specialty = dto.employee.specialty,
            };
        }

        public static Employee RegisterUserReqDtoToEmployeeEntity(RegisterUserReqDto dto)
        {
            List<TimeOnly> workStartTimes = new List<TimeOnly>();
            List<TimeOnly> workEndTimes = new List<TimeOnly>();

            for (int i = 0 ; i < dto.employee.workStartTimes.Count ; i++)
            {
                workStartTimes.Add(Util.stringTimeToTime(dto.employee.workStartTimes[i]));
                workEndTimes.Add(Util.stringTimeToTime(dto.employee.workEndTimes[i]));
            }

            return new Employee
            {
                Email = dto.user.email,
                Password = dto.user.password,
                FirstName = dto.user.firstName,
                LastName = dto.user.lastName,
                DateOfBirth = Util.stringDateToDate(dto.user.dateOfBirth),
                Nationality = dto.user.nationality,
                Gender = dto.user.gender,
                PhoneCountryCode = dto.user.phoneCountryCode,
                PhoneNumber = dto.user.phoneNumber,
                UserRoles = dto.userRoles,
                PhoneExt = dto.employee.phoneExt,
                OfficeRoomNumber = dto.employee.officeRoomNo,
                WorkStartTimes = workStartTimes,
                WorkEndTimes = workEndTimes
            };
        }
    }
}
