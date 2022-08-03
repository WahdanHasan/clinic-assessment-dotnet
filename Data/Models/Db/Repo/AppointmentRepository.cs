using clinic_assessment.data;
using clinic_assessment_redone.Api.Dto.Appointment;
using clinic_assessment_redone.Data.Models.Db.Entity;
using clinic_assessment_redone.Data.Models.Db.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace clinic_assessment_redone.Data.Models.Db.Repo
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DataContext _dataContext;
        public AppointmentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Appointment>> GetAll()
        {
            return await _dataContext.Appointment.ToListAsync();
        }

        public async Task<List<Appointment>> GetAllByDoctorId(DateOnly appointmentHistoryDate, int doctorId)
        {
            List<Appointment> doctorAppointments = await _dataContext.Appointment
                .Include(a => a.Patient)
                .Where(a => a.DateCreated.Equals(appointmentHistoryDate))
                .ToListAsync();

            return doctorAppointments;
        }

        public async Task<List<Appointment>> GetAppointmentsByDate(DateOnly appointmentDate, string status)
        {
            List<Appointment> appointmentsList = await _dataContext.Appointment
                .Include(a => a.Patient)
                 .Where(a => a.DateCreated.Equals(appointmentDate)
                 && a.Status.Equals(status)
                 )
                 .ToListAsync();


            return appointmentsList;
        }

        public async Task<List<Appointment>> GetAppointmentsForDayByDoctorId(DateOnly appointmentDate, int doctorId, string status)
        {
            List<Appointment> appointmentsList = await _dataContext.Appointment
                .Where(a => a.DateCreated.Equals(appointmentDate)
                && a.Doctor.Id == doctorId 
                && a.Status.Equals(status)
                )
                .ToListAsync();


            return appointmentsList;
        }

        public async Task<List<Appointment>> GetAppointmentsForDayByPatientId(DateOnly appointmentDate, int patientId, string status)
        {
            List<Appointment> appointmentsList = await _dataContext.Appointment
                .Where(a => a.DateCreated.Equals(appointmentDate)
                && a.Patient.Id == patientId
                && a.Status.Equals(status)
                )
                .ToListAsync();


            return appointmentsList;
        }

        public async Task<Appointment> GetById(int id)
        {
            return await _dataContext.Appointment.Where(a => a.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<AppointmentDetailsResDto> GetDetailsById(int appointmentId)
        {
            AppointmentDetailsResDto res = await _dataContext.Appointment
                .Where(a => a.Id == appointmentId)
                .Select(a => new AppointmentDetailsResDto
                {
                    doctorId = a.Doctor.Id,
                    doctorFullName = a.Doctor.FirstName + " " + a.Doctor.LastName,
                    date = a.DateCreated,
                    timeStart = a.TimeStart,
                    timeEnd = a.TimeEnd,
                    patientAttended = a.PatientAttended,
                    status = a.Status
                })
                .FirstOrDefaultAsync();

            return res;
        }

        public async Task<PatientAppointmentsResDto> GetPatientAptHistory(int patientId)
        {
            var patientAppointmentList = await _dataContext.Patient
                .Select(p => new PatientAppointmentsResDto
                {
                    patientId = patientId,
                    patientFirstName = p.FirstName,
                    patientLastName = p.LastName,
                    appointments = _dataContext.Appointment
                    .Select(a => new AppointmentDetailsResDto
                    {
                        doctorId = a.Doctor.Id,
                        doctorFullName = a.Doctor.FirstName + " " + a.Doctor.LastName,
                        date = a.DateCreated,
                        timeStart = a.TimeStart,
                        timeEnd = a.TimeEnd,
                        durationMins = a.DurationMins,
                        patientAttended = a.PatientAttended,
                        status = a.Status
                    }).ToList()
                })
                .Where(res => res.patientId == patientId)
                .FirstOrDefaultAsync();

            return patientAppointmentList;
        }

        public async Task<Appointment> Insert(Appointment entity)
        {
            await _dataContext.Appointment.AddAsync(entity);
            return entity;
        }

        public Task Save()
        {
            return _dataContext.SaveChangesAsync();
        }
    }
}
