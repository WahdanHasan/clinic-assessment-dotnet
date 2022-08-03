using clinic_assessment_redone.Api.Dto.Appointment;
using clinic_assessment_redone.Data.Models.Db.Entity;

namespace clinic_assessment_redone.Helpers.Controllers.Interfaces
{
    public interface IAppointmentHelper
    {
        Task<IEnumerable<Appointment>> GetAll();
        Task<PatientAppointmentsResDto> GetAllPatientAppointments(int patientId);
        Task CancelAppointment(int appointmentId);
        Task<AppointmentDetailsResDto> GetAppointmentDetails(int appointmentId);
        Task BookAppointment(BookAppointmentReqDto req);
    }
}
