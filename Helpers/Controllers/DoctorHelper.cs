using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment.data.Models.Db.Repo.Interfaces;
using clinic_assessment_redone.Api.Dto.Doctor;
using clinic_assessment_redone.Data.Models.Db.Entity;
using clinic_assessment_redone.Data.Models.Db.Repo.Interfaces;
using clinic_assessment_redone.Helpers.Controllers.Interfaces;
using clinic_assessment_redone.Helpers.Dto;
using clinic_assessment_redone.Helpers.Mapper;
using clinic_assessment_redone.Helpers.Misc;
using clinic_assessment_redone.Middleware.ErrorHandler.Exceptions;

namespace clinic_assessment_redone.Helpers.Controllers
{
    public class DoctorHelper : IDoctorHelper
    {
        private readonly IRepository<Doctor, int> _doctorRepository;
        private readonly IRepository<Appointment, int> _appointmentRepository;
        private readonly IRepository<Patient, int> _patientRepository;

        public DoctorHelper(IRepository<Doctor, int> doctorRepository, IRepository<Appointment, int> appointmentRepository, IRepository<Patient, int> patientRepository)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
        }

        public async Task<AllDoctorsResDto> GetAllDoctors()
        {
            /* Get all doctors from DB */
            var doctors = await _doctorRepository.GetAll();

            /* Map doctor entity list to DTO */
            AllDoctorsResDto dto = new AllDoctorsResDto();
            List<DoctorResDto> doctorsList = new List<DoctorResDto>();
            dto.doctors = doctorsList;

            foreach (var doctor in doctors)
            {
                doctorsList.Add(DoctorMapper.doctorEntityToGetDoctorResDtoDto(doctor));

            }

            return dto;
        }

        public async Task<AllBusyDoctorsByAptCountResDto> GetAllBusyByAptCount(string date)
        {
            /* Ensure the provided date is in a valid format */
            DateOnly queryDate = Util.stringDateToDate(date);

            /* Get all doctors with their appointment count*/
            var busyDoctorsList = await ((IDoctorRepository)_doctorRepository).GetBusyDoctorsByAptCount(queryDate);

            /* Sort list of doctors by total appointment count */

            /* Initialize response DTO */
            AllBusyDoctorsByAptCountResDto dto = new AllBusyDoctorsByAptCountResDto();
            dto.doctors = busyDoctorsList;

            return dto;
        }

        public async Task<AllBusyDoctorsByMinHoursResDto> GetAllBusyByMinHours(string date, int minHours)
        {
            /* Ensure the provided date is in a valid format */
            DateOnly queryDate = Util.stringDateToDate(date);

            /* Get all doctors; filtered by the min appointment hours */
            List<BusyDoctorsByMinHoursResDto> busyDoctorsList = await ((IDoctorRepository)_doctorRepository).GetBusyDoctorsByMinHours(queryDate, minHours);

            /* Initialize response DTO */
            AllBusyDoctorsByMinHoursResDto dto = new AllBusyDoctorsByMinHoursResDto();
            dto.doctors = busyDoctorsList;

            return dto;
        }

        public async Task<DoctorAvailableSlotResDto> GetAvailableSlots(string date, int doctorId)
        {
            /* Validate that the doctor exists */
            Doctor doctor = await ((IDoctorRepository)_doctorRepository).GetById(doctorId);

            if (doctor == null)
            {
                throw new RestException(StatusCodes.Status400BadRequest, String.Format(Constants.NOT_FOUND, doctorId));
            }

            DateOnly appointmentHistoryDate = DateOnly.Parse(date);

            /* Get all appointments on the provided date and doctor id */
            List<Appointment> appointmentList = await ((IAppointmentRepository)_appointmentRepository).GetAllByDoctorId(appointmentHistoryDate, doctorId);

            List<TimeOnly> appointmentStartTimesList = Util.GetStartTimesFromAppointmentList(appointmentList);
            List<TimeOnly> appointmentEndTimesList = Util.GetEndTimesFromAppointmentList(appointmentList);

            /* If user has patient role, limit access by not adding certain information */
            // TODO: 

            List<int> patientIdList = Util.GetPatientIdsFromAppointmentList(appointmentList);

            if (patientIdList == null)
            {
                return new DoctorAvailableSlotResDto();
            }

            /* Get patient full names in dictionary */
            Dictionary<int, string> patientIdToFullNames = await ((IPatientRepository)_patientRepository).GetFullNamesByIds(patientIdList);

            
            List<BasicAppointmentDetails> occupiedSlots = Util.GetDoctorOccupiedSlots(appointmentStartTimesList, appointmentEndTimesList, patientIdList);

            /* Add patient info to occupied slots */
            foreach(BasicAppointmentDetails aptDetails in occupiedSlots)
            {
                aptDetails.patientFullName = patientIdToFullNames[aptDetails.patientId.Value];
            }

            /* Get all available slots */
            List<BasicAppointmentDetails> availableSlots = Util.GetDoctorAvailableSlots(doctor.WorkStartTimes,
                                                                                        doctor.WorkEndTimes,
                                                                                        appointmentStartTimesList,
                                                                                        appointmentEndTimesList);

            /* Set values for response DTO */
            DoctorAvailableSlotResDto dto = new DoctorAvailableSlotResDto();
            dto.doctorId = doctor.Id;
            dto.doctorFullName = doctor.FirstName + doctor.LastName;
            dto.availableSlots = availableSlots;
            dto.occupiedSlots = occupiedSlots;


            return dto;
        }

        public async Task<List<DoctorAvailableSlotResDto>> GetAllAvailableSlots(string date)
        {
            /* Get all appointments for the date */
            List<Appointment> appointmentsList = await ((IAppointmentRepository)_appointmentRepository)
                    .GetAppointmentsByDate(Util.stringDateToDate(date), Constants.APPOINTMENT_STATUS_VALID);


            /* Get all doctor entities */
            List<Doctor> doctorEntityList = (await _doctorRepository.GetAll()).ToList();


            /* Create a list of all doctor ids */
            List<int> doctorIds = new List<int>();

            doctorEntityList.ForEach((doctor)=> 
                doctorIds.Add(doctor.Id)
            );


            /* Create map of doctor id to doctor full name */
            Dictionary<int, string> doctorIdToFullNameMap = new Dictionary<int, string>();
            string tempFullName;
            foreach (Doctor doctorEntity in doctorEntityList)
            {
                tempFullName = doctorEntity.FirstName + doctorEntity.LastName;

                doctorIdToFullNameMap.Add(doctorEntity.Id, tempFullName);
            }

            ///* Get user roles */
            //List<String> userRoles = roleRepo.getUserRolesFromEmail(userEmail);

            /* If the user has the role of patient, then return a list of available doctors */
            List<DoctorAvailableSlotResDto> dtoList = new List<DoctorAvailableSlotResDto>();
            DoctorAvailableSlotResDto dtoElement;

            //if (userRoles.contains(Constants.ROLE_PATIENT))
            //{

            //    /* Create a map of the total appointment count and hours per doctor id */
            //    Map<Integer, Integer> doctorIdToAppointmentCountMap = new HashMap<>();
            //    Map<Integer, Double> doctorIdToAppointmentHoursMap = new HashMap<>();

            //    Integer tempDoctorId;
            //    Integer tempAppointmentCount;
            //    Double tempAppointmentHours;
            //    for (AppointmentEntity appointment : appointmentsList)
            //    {
            //        tempDoctorId = appointment.getDoctor().getId();

            //        tempAppointmentCount = doctorIdToAppointmentCountMap.get(tempDoctorId);

            //        /* If key exists, add 1 to its value, else, create the key and set the value to 0 */
            //        doctorIdToAppointmentCountMap.put(tempDoctorId,
            //                                      (tempAppointmentCount == null) ? 0 : tempAppointmentCount + 1);

            //        tempAppointmentHours = doctorIdToAppointmentHoursMap.get(tempDoctorId);

            //        /* If key exists, add the appointment hours to its value, else, cre*ate the key and set the value to 0 */
            //        doctorIdToAppointmentHoursMap.put(tempDoctorId,
            //                (tempAppointmentHours == null) ? 0.00
            //                        : tempAppointmentHours + (appointment.getDurationMins() / (double)Constants.MINUTES_IN_HOUR)
            //        );
            //    }

            //    /* If the doctor's total appointment count is below the threshold and if the doctor's total appointment hours
            //    *  is less than the threshold with enough time for a minimum duration appointment, then add the doctor's name
            //    *  to the list of available doctors.
            //    *  */
            //    for (Integer doctorId : doctorIds)
            //    {
            //        getDoctorAvailableSlotRespDto = new GetDoctorAvailableSlotRespDto();
            //        getDoctorAvailableSlotRespDtoList.add(getDoctorAvailableSlotRespDto);

            //        /* If the doctor has no scheduled appointments for the day, add his name */
            //        if (doctorIdToAppointmentCountMap.get(doctorId) == null || doctorIdToAppointmentHoursMap.get(doctorId) == null)
            //        {
            //            getDoctorAvailableSlotRespDto.setDoctorFullName(doctorIdToFullNameMap.get(doctorId));
            //            continue;
            //        }

            //        /* If the doctor has schedule appointments, and they're below the limits, add his name */
            //        if (doctorIdToAppointmentCountMap.get(doctorId) < appointmentMaxCount
            //        && doctorIdToAppointmentHoursMap.get(doctorId) < (appointMaxDuration - appointmentMinDuration))
            //        {

            //            getDoctorAvailableSlotRespDto.setDoctorFullName(doctorIdToFullNameMap.get(doctorId));

            //        }
            //    }

            //    return getDoctorAvailableSlotRespDtoList;
            //}


            /* If the user is not a patient, proceed with getting more detailed information */

            /* Get a list of patients ids from the appointment list */
            HashSet<int> patientIdSet = new HashSet<int>();
            Dictionary<int, string> patientIdToFullNameMap = new Dictionary<int, string>();

            appointmentsList.ForEach((appointment) =>
                patientIdSet.Add(appointment.Patient.Id)
                );

            List<int> patientIdsForLookup = new List<int>(patientIdSet);

            /* Get the patient entities that correspond to the patient ids */

            List<Patient> patientEntityList = await ((IPatientRepository)_patientRepository).GetAllByIdList(patientIdsForLookup);

            /* Create map for patient id to patient full name */
            foreach (Patient patient in patientEntityList)
            {
                patientIdToFullNameMap.Add(patient.Id,
                        patient.FirstName + patient.LastName);
            }

            /* Get occupied and available slots for all doctors and map them to the response DTO */
            List<TimeOnly> appointmentStartTimesList;
            List<TimeOnly> appointmentEndTimesList;
            List<int> patientIdList = null;
            List<BasicAppointmentDetails> availableSlots;
            List<BasicAppointmentDetails> occupiedSlots;

            foreach (Doctor doctor in doctorEntityList)
            {
                /* Set already available fields */
                dtoElement = new DoctorAvailableSlotResDto();
                dtoElement.doctorFullName = doctorIdToFullNameMap[doctor.Id];
                dtoElement.doctorId = doctor.Id;
                dtoList.Add(dtoElement);


                /* Create a reference list to all appointment start and end times */
                appointmentStartTimesList = Util.GetAppointmentStartTimesFromEntityList(appointmentsList, doctor.Id);
                appointmentEndTimesList = Util.GetAppointmentEndTimesFromEntityList(appointmentsList, doctor.Id);
                patientIdList = Util.GetPatientIdsFromEntityList(appointmentsList, doctor.Id);


                /* Get doctor occupied and available slots */
                occupiedSlots = Util.GetDoctorOccupiedSlots(appointmentStartTimesList,
                        appointmentEndTimesList,
                        patientIdList);

                availableSlots = Util.GetDoctorAvailableSlots(doctor.WorkStartTimes,
                        doctor.WorkEndTimes,
                        appointmentStartTimesList,
                        appointmentEndTimesList);

                /* Set patient full names for occupied slots */
                foreach (BasicAppointmentDetails occupiedSlot in occupiedSlots)
                {
                    occupiedSlot.patientFullName = patientIdToFullNameMap[occupiedSlot.patientId.Value];
                }

                dtoElement.availableSlots = availableSlots;
                dtoElement.occupiedSlots = occupiedSlots;

            }

            return dtoList;
        }

        public async Task<DoctorInfoDto> GetDoctor(int doctorId)
        {
            /* Get the dto */
            DoctorInfoDto dto = await ((IDoctorRepository)_doctorRepository).GetInfoById(doctorId);

            /* Validate that the doctor exists */
            if (dto == null)
            {
                throw new RestException(StatusCodes.Status400BadRequest, String.Format(Constants.NOT_FOUND, doctorId));
            }
            
            /* Extrapolate work schedule from the available information */
            List<string> workSchedule = new List<string>();

            for(int i = 0; i < dto.workStartTimes.Count; i++)
            {
                workSchedule.Add(dto.workStartTimes[i] + "-" + dto.workEndTimes[i]);
            }

            /* Set properties */
            dto.workSchedule = workSchedule;


            return dto;

        }
    }
}
