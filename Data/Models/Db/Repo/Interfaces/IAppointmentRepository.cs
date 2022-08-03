using clinic_assessment.data.Models.Db.Repo.Interfaces;
using clinic_assessment_redone.Api.Dto.Appointment;
using clinic_assessment_redone.Data.Models.Db.Entity;

namespace clinic_assessment_redone.Data.Models.Db.Repo.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment, int>
    {
        Task<PatientAppointmentsResDto> GetPatientAptHistory(int patientId);
        Task<List<Appointment>> GetAllByDoctorId(DateOnly appointmentHistoryDate, int doctorId);
        Task<List<Appointment>> GetAppointmentsForDayByDoctorId(DateOnly appointmentDate, int doctorId, string status);
        Task<List<Appointment>> GetAppointmentsForDayByPatientId(DateOnly appointmentDate, int patientId, string status);
        Task<List<Appointment>> GetAppointmentsByDate(DateOnly appointmentDate, string status);
        Task<AppointmentDetailsResDto> GetDetailsById(int appointmentId);
    }
}
