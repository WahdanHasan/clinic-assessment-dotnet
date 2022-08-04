using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment_redone.Api.Dto.Appointment;
using clinic_assessment_redone.Data.Models.Db.Entity;
using clinic_assessment_redone.Helpers.Misc;

namespace clinic_assessment_redone.Helpers.Mapper
{
    public class AppointmentMapper
    {
        internal static Appointment BookAppointmentReqDtoToAppointmentEntity(BookAppointmentReqDto req, Patient patient, Doctor doctor)
        {
            return new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                DateCreated = Util.stringDateToDate(req.appointmentDate),
                TimeStart = Util.stringTimeToTime(req.appointmentTimeStart),
                TimeEnd = Util.stringTimeToTime(req.appointmentTimeEnd),
                Status = Consts.APPOINTMENT_STATUS_VALID
            };
        }

        public static AppointmentDetailsResDto AppointmentEntityToAppointmentDetailsResDto(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}
