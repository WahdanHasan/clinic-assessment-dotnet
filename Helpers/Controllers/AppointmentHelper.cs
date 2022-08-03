using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using clinic_assessment_redone.Api.Dto.Appointment;
using clinic_assessment_redone.Data.Models.Db.Entity;
using clinic_assessment_redone.Data.Models.Db.Repo.Interfaces;
using clinic_assessment_redone.Helpers.Controllers.Interfaces;
using clinic_assessment_redone.Helpers.Mapper;
using clinic_assessment_redone.Helpers.Misc;
using clinic_assessment_redone.Middleware.ErrorHandler.Exceptions;

namespace clinic_assessment_redone.Helpers.Controllers
{
    public class AppointmentHelper : IAppointmentHelper
    {
        private readonly IRepository<Appointment, int> _appointmentRepository;
        private readonly IRepository<Doctor, int> _doctorRepository;
        private readonly IRepository<Patient, int> _patientRepository;

        public AppointmentHelper(IRepository<Appointment, int>  appointmentRepository, IRepository<Doctor, int> doctorRepository, IRepository<Patient, int> patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
        }

        public async Task BookAppointment(BookAppointmentReqDto req)
        {
            /* Ensure that the doctor and patient provided exist */
            // Validate doctor
            Doctor doctor = await ((IDoctorRepository)_doctorRepository).GetById(req.doctorId);

            if (doctor == null)
            {
                throw new RestException(StatusCodes.Status400BadRequest, String.Format(Constants.NOT_FOUND, req.doctorId));
            }

            // Validate patient
            Patient patient = await ((IPatientRepository)_patientRepository).GetById(req.patientId);

            if (patient == null)
            {
                throw new RestException(StatusCodes.Status400BadRequest, String.Format(Constants.NOT_FOUND, req.patientId));

            }

            /* Ensure that the appointment date and time provided are valid */
            /* Validate date format and ensure it does not exceed the max duration*/

            DateOnly appointmentDate = Util.stringDateToDate(req.appointmentDate);
            DateOnly maxAppointmentDate = DateOnly.FromDateTime(DateTime.Now).AddDays(Constants.APPOINTMENT_LOOKAHEAD_DAYS);
            DateOnly yesterdayDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-1);

            // If appointment date is after the max date or before the min date
            if (appointmentDate > maxAppointmentDate || appointmentDate < yesterdayDate)
            {
                throw new RestException(
                        StatusCodes.Status400BadRequest,
                        String.Format(
                                Constants.FIELD_OUTSIDE_RANGE,
                                "appointmentDate",
                                "up to " + Constants.APPOINTMENT_LOOKAHEAD_DAYS + " days ahead from current date"));
            }


            /* Validate time format */
            TimeOnly appointmentTimeStart;
            TimeOnly appointmentTimeEnd;

            appointmentTimeStart = Util.stringTimeToTime(req.appointmentTimeStart);
            appointmentTimeEnd = Util.stringTimeToTime(req.appointmentTimeEnd);

            /* DISCLAIMER: The validation to ensure the appointment start time hasn't already passed was left out on
            *  purpose as testing/using the application would be quite annoying to do.
            *
            *   */

            if (appointmentTimeStart > appointmentTimeEnd)
            {
                throw new RestException(
                        StatusCodes.Status400BadRequest,
                        String.Format(Constants.TIME_OCCURS_TIME_DATE, appointmentTimeStart.ToString(), appointmentTimeEnd.ToString())
                );
            }


            /* Ensure the appointment duration is within limits */
            int appointmentDurationMins = Util.MinutesBetweenTimes(appointmentTimeStart, appointmentTimeEnd);
            if (appointmentDurationMins > Constants.APPOINTMENT_MAX_MINUTES
                    || appointmentDurationMins < Constants.APPOINTMENT_MIN_MINUTES)
            {

                throw new RestException(
                        StatusCodes.Status400BadRequest,
                        String.Format(
                                Constants.FIELD_OUTSIDE_RANGE,
                                "appointmentDurationMins",
                                Constants.APPOINTMENT_MIN_MINUTES + " to " + Constants.APPOINTMENT_MAX_MINUTES + " mins")
                );
            }


            /* Ensure the appointment time is within the doctor's scheduled hours */
            bool appointmentBetweenSchedule = false;
            TimeOnly tempScheduleTimeStart;
            TimeOnly tempScheduleTimeEnd;
            for (int i = 0; i < doctor.WorkStartTimes.Count; i++)
            {
                tempScheduleTimeStart = doctor.WorkStartTimes[i];
                tempScheduleTimeEnd = doctor.WorkEndTimes[i];

                /* If the appointment start and end time are outside the current schedule, check the next doctor schedule */
                /* This is based on the understanding that some doctors sit a certain amount of hours at a hospital/clinic
                *  and then leave. They may have multiple scheduled hours at the hospital/clinic. */
                if (appointmentTimeStart < tempScheduleTimeStart
                        || appointmentTimeStart > tempScheduleTimeEnd)
                {
                    continue;
                }

                /* If the appointment was inside one of the doctor's sitting hours, break */
                appointmentBetweenSchedule = true;
                break;
            }

            if (!appointmentBetweenSchedule)
            {
                throw new RestException(StatusCodes.Status400BadRequest, Constants.OUTSIDE_DOCTOR_SCHEDULE);
            }


            /* Ensure the doctor has not exceeded his total appointment count or hours */
            List<Appointment> doctorsAppointments = await ((IAppointmentRepository)_appointmentRepository)
                    .GetAppointmentsForDayByDoctorId(appointmentDate,
                                           doctor.Id,
                                           Constants.APPOINTMENT_STATUS_VALID);

            /* Check appointment count */
            if (doctorsAppointments.Count > Constants.APPOINTMENT_MAX_COUNT)
            {
                throw new RestException(
                        StatusCodes.Status503ServiceUnavailable,
                        String.Format(
                                Constants.DOCTOR_FULL_SCHEDULE,
                                req.appointmentDate)
                );
            }

            /* Check appointment total hours */
            int totalAppointmentDurationMins = 0;
            foreach(Appointment doctorAppointment in doctorsAppointments)
            {
                totalAppointmentDurationMins += doctorAppointment.DurationMins;
            }

            if (totalAppointmentDurationMins > Constants.APPOINTMENT_MAX_TOTAL_MINUTES)
            {
                throw new RestException(
                        StatusCodes.Status503ServiceUnavailable,
                        String.Format(
                                Constants.DOCTOR_FULL_SCHEDULE,
                                req.appointmentDate)
                );
            }

            /* Ensure the requested appointment time period does not overlap with another appointment */
            TimeOnly tempAppointmentTimeStart;
            TimeOnly tempAppointmentTimeEnd;
            foreach (Appointment doctorAppointment in doctorsAppointments)
            {
                /* Parse string times to local times. Add/Subtract 1 minute to make ensure it does not overlap */
                tempAppointmentTimeStart = doctorAppointment.TimeStart;
                tempAppointmentTimeEnd = doctorAppointment.TimeEnd;

                /* If either the appointment start or end time are in between the already booked appointment start and end time.
                *  Or if the appointment start or end time are 1:1 with the already booked appointment start and end time,
                *  raise exception.
                * */
                if (((appointmentTimeStart > tempAppointmentTimeStart && appointmentTimeStart < tempAppointmentTimeEnd)
                    || (appointmentTimeEnd > tempAppointmentTimeStart && appointmentTimeEnd < tempAppointmentTimeEnd))
                || (appointmentTimeStart == tempAppointmentTimeStart || appointmentTimeEnd == tempAppointmentTimeEnd))
                {
                    throw new RestException(StatusCodes.Status400BadRequest, Constants.DOCTOR_UNAVAILABLE_DURING_HOURS);
                }
            }


            /* Ensure the patient does not have overlapping appointments */
            List<Appointment> patientAppointments = await ((IAppointmentRepository)_appointmentRepository).
                    GetAppointmentsForDayByPatientId(appointmentDate,
                                                     req.patientId,
                                                     Constants.APPOINTMENT_STATUS_VALID);

            foreach (Appointment patientAppointment in patientAppointments)
            {
                /* Parse string times to local times. Add/Subtract 1 minute to make ensure it does not overlap */
                tempAppointmentTimeStart = patientAppointment.TimeStart.AddMinutes(-1);
                tempAppointmentTimeEnd = patientAppointment.TimeEnd.AddMinutes(1);

                /* Check if the requested time is between another appointment's scheduled time */
                if (appointmentTimeStart > tempAppointmentTimeStart && appointmentTimeStart < tempAppointmentTimeEnd
                        || appointmentTimeEnd > tempAppointmentTimeStart && appointmentTimeEnd < tempAppointmentTimeEnd)
                {
                    throw new RestException(StatusCodes.Status400BadRequest, Constants.PATIENT_OVERLAPPING_APPOINTMENTS);
                }
            }

            /* Map the request DTO to an appointment entity */
            Appointment appointment = AppointmentMapper.BookAppointmentReqDtoToAppointmentEntity(req, patient, doctor);
            appointment.DurationMins = appointmentDurationMins;

            await _appointmentRepository.Insert(appointment);
            await _appointmentRepository.Save();

            return;
        }

        public async Task CancelAppointment(int appointmentId)
        {

            /* Get appointment from DB by provided id */
            Appointment appointment = await ((IAppointmentRepository)_appointmentRepository).GetById(appointmentId);

            /* If appointment is null, throw exception that the appointment doesnt exist */
            if (appointment == null)
            {
                throw new RestException(StatusCodes.Status400BadRequest, Constants.NOT_FOUND);
            }

            /* Update the appointment status */
            appointment.Status = Constants.APPOINTMENT_STATUS_CANCELLED;

            /* Persist changes to DB */
            await ((IAppointmentRepository)_appointmentRepository).Insert(appointment);

            await ((IAppointmentRepository)_appointmentRepository).Save();

        }

        public async Task<IEnumerable<Appointment>> GetAll()
        {
            return await _appointmentRepository.GetAll();
        }

        public async Task<PatientAppointmentsResDto> GetAllPatientAppointments(int patientId)
        {
            PatientAppointmentsResDto patientAppointments = await ((IAppointmentRepository)_appointmentRepository).GetPatientAptHistory(patientId);

            return patientAppointments;
        }

        public async Task<AppointmentDetailsResDto> GetAppointmentDetails(int appointmentId)
        {
            /* Get appointment from DB by provided id */
            AppointmentDetailsResDto dto = await ((IAppointmentRepository)_appointmentRepository).GetDetailsById(appointmentId);

            /* If appointment is null, throw exception that the appointment doesnt exist */
            if (dto == null)
            {
                throw new RestException(StatusCodes.Status400BadRequest, String.Format(Constants.NOT_FOUND, appointmentId));
            }

            dto.durationMins = Util.MinutesBetweenTimes(dto.timeStart, dto.timeEnd);

            return dto;
        }
    }
}
