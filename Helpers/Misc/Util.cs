using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment_redone.Api.Dto.User;
using clinic_assessment_redone.Data.Models.Db.Entity;
using clinic_assessment_redone.Helpers.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace clinic_assessment_redone.Helpers.Misc
{
    public static class Util
    {
        public static DateTime UnixTimestampToUTC(int timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static string CreateToken(string userPrincipal, IConfiguration configuration, List<Claim> userClaims, Nullable<DateTime> expiry)
        {

            // Add principal claim
            userClaims.Add(new Claim(ClaimTypes.Name, userPrincipal));


            /* Sign token */
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:SecretKey").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: userClaims,
                expires: expiry,
                signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public static string hashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool verifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static DateOnly stringDateToDate(string date)
        {
            return DateOnly.ParseExact(date, Consts.DATE_FORMAT, null);
        }
        public static TimeOnly stringTimeToTime(string time)
        {
            return TimeOnly.ParseExact(time, Consts.TIME_FORMAT, null);
        }

        public static bool roleHasPermission(RolePermissionsDto rolePermission, string permissionName)
        {
            foreach(Permission perm in rolePermission.permissions)
            {
                if (perm.Name.Equals(permissionName)){
                    return true;
                }
            }

            return false;
        }

        public static bool roleHasPermission(List<RolePermissionsDto> rolePermissions, string permissionName, string roleName)
        {
            foreach(RolePermissionsDto rolePermission in rolePermissions)
            {
                if (rolePermission.roleName.Equals(roleName))
                {
                    return roleHasPermission(rolePermission, permissionName);
                }
            }

            return false;
        }

        public static bool containsMultipleUniqueRoles(IEnumerable<string> roles)
        {
            bool containsRoleDoctor = roles.Contains(Consts.ROLE_DOCTOR);
            bool containsRolePatient = roles.Contains(Consts.ROLE_PATIENT);
            bool containsRoleCA = roles.Contains(Consts.ROLE_CA);

            if (containsRoleDoctor && containsRolePatient
                || containsRoleDoctor && containsRoleCA
                || containsRolePatient && containsRoleCA
                )
            {
                return true;
            }

            return false;
        }

        public static void validateUserAttributes(UserDetails userDetails)
        {

            // TODO: do validate
        }

        public static void validateEmployeeAttributes(GetDoctorResDtoDto employeeDetails)
        {

            // TODO: do validate
        }

        public static void validatePatientAttributes(PatientDetails patientDetails)
        {

            // TODO: do validate
        }

        public static int MinutesBetweenTimes(TimeOnly t1, TimeOnly t2)
        {
            int hours = t1.Hour - t2.Hour;
            int minutes = t1.Minute - t2.Minute;

            return (Math.Abs(hours) * Consts.MINUTES_IN_HOUR)
                  + Math.Abs(minutes);
        }
        public static List<TimeOnly> GetStartTimesFromAppointmentList(List<Appointment> appointmentList)
        {
            List<TimeOnly> appointmentStartTimesList = new List<TimeOnly>();

            foreach(Appointment appointment in appointmentList)
            {
                appointmentStartTimesList.Add(appointment.TimeStart);
            }

            return appointmentStartTimesList;
        }

        public static List<int> GetPatientIdsFromAppointmentList(List<Appointment> appointmentList)
        {
            List<int> patientIds = new List<int>();

            foreach(Appointment appointment in appointmentList)
            {
                patientIds.Add(appointment.Patient.Id);
            }

            return patientIds;
        }

        public static List<BasicAppointmentDetails> GetDoctorOccupiedSlots(List<TimeOnly> appointmentStartTimesList, List<TimeOnly> appointmentEndTimesList, List<int> patientIdList)
        {
            List<BasicAppointmentDetails> occupiedSlots = new List<BasicAppointmentDetails>(appointmentStartTimesList.Count);

            BasicAppointmentDetails tempDetails;
            for(int i = 0; i < appointmentStartTimesList.Count; i++)
            {
                tempDetails = new BasicAppointmentDetails();

                if (patientIdList != null)
                {
                    tempDetails.patientId = patientIdList[i];
                }
                tempDetails.timeStart = appointmentStartTimesList[i].ToString();
                tempDetails.timeEnd = appointmentEndTimesList[i].ToString();

                occupiedSlots.Add(tempDetails);
            }

            return occupiedSlots;
        }

        public static List<BasicAppointmentDetails> GetDoctorAvailableSlots(List<TimeOnly> doctorStartTimesList, List<TimeOnly> doctorEndTimesList, List<TimeOnly> appointmentStartTimesList, List<TimeOnly> appointmentEndTimesList)
        {
            /* Instantiate a list of booleans to ensure appointments are not checked multiple times in an algorithm written later in this function */
            List<bool> appointmentChecked = new List<bool>(appointmentStartTimesList.Count);

            for (int i = 0; i < appointmentStartTimesList.Count; i++)
            {
                appointmentChecked.Add(false);
            }


            /* Get a doctor's available slots
            *
            *  This algorithm works by moving the 'timeStart' and 'timeEnd' time pointers in between the empty time periods
            *  between the doctor's scheduled hours and appointments. If the empty time period they are set to have a duration
            *  that exceeds the minimum appointment time, this empty time period is marked as an available slot.
            *  */
            List<BasicAppointmentDetails> doctorAvailableSlots = new List<BasicAppointmentDetails>();

            TimeOnly timeStart;
            TimeOnly timeEnd;
            TimeOnly tempTimeStart;
            long timeDuration;
            BasicAppointmentDetails tempDoctorAvailableSlot;
            for (int i = 0; i < doctorStartTimesList.Count; i++)
            {

                timeStart = doctorStartTimesList[i];

                for (int j = 0; j < appointmentStartTimesList.Count; j++)
                {

                    /* If this appointment has been checked. Check the next */
                    if (appointmentChecked[j] == true)
                    {
                        continue;
                    }

                    /* If this appointment starts after this work time period ends, check the next work time */
                    if (appointmentStartTimesList[j] > doctorEndTimesList[i])
                    {
                        continue;
                    }

                    /* Calculate duration of time in the slot */
                    timeEnd = appointmentStartTimesList[j];

                    timeDuration = MinutesBetweenTimes(timeStart, timeEnd);

                    /* Set the start of the slot we're looking at to the next gap */
                    tempTimeStart = timeStart;
                    timeStart = appointmentEndTimesList[j];

                    /* If the slot is less than the minimum appointment time, it cannot be utilized.
                    *  Else, set this as an available slot
                    *
                    *  */
                    if (timeDuration < Consts.APPOINTMENT_MIN_MINUTES && timeDuration > 0)
                    {
                        continue;
                    }
                    else if (timeDuration < 1)
                    {
                        appointmentChecked[j] = true;
                        continue;
                    }

                    /* Set the appointment start time to null as we've taken this appointment into account */
                    appointmentChecked[j] = true;

                    /* Add the slot to the list of available slots */
                    tempDoctorAvailableSlot = new BasicAppointmentDetails();

                    tempDoctorAvailableSlot.timeStart = tempTimeStart.ToString();
                    tempDoctorAvailableSlot.timeEnd = timeEnd.ToString();
                    doctorAvailableSlots.Add(tempDoctorAvailableSlot);
                }


                /* Calculate the leftover slot after the doctor's last appointment and before his schedule end */
                timeEnd = doctorEndTimesList[i];

                timeDuration = MinutesBetweenTimes(timeStart, timeEnd);

                if (timeDuration > Consts.APPOINTMENT_MIN_MINUTES)
                {
                    tempDoctorAvailableSlot = new BasicAppointmentDetails();

                    tempDoctorAvailableSlot.timeStart = timeStart.ToString();
                    tempDoctorAvailableSlot.timeEnd = timeEnd.ToString();

                    doctorAvailableSlots.Add(tempDoctorAvailableSlot);
                }
            }

            return doctorAvailableSlots;

        }

        public static List<TimeOnly> GetEndTimesFromAppointmentList(List<Appointment> appointmentList)
        {
            List<TimeOnly> appointmentEndTimesList = new List<TimeOnly>();

            foreach (Appointment appointment in appointmentList)
            {
                appointmentEndTimesList.Add(appointment.TimeEnd);
            }

            return appointmentEndTimesList;
        }

        public static List<TimeOnly> GetAppointmentStartTimesFromEntityList(List<Appointment> appointmentsList, int doctorId)
        {
            /* Get the appointment start times from an appointment entity list */

            List<TimeOnly> appointmentStartTimesList = new List<TimeOnly>();

            foreach (Appointment appointmentEntity in appointmentsList)
            {
                if (doctorId == -1)
                {
                    appointmentStartTimesList.Add(appointmentEntity.TimeStart);
                }
                else if (doctorId == appointmentEntity.Doctor.Id)
                {
                    appointmentStartTimesList.Add(appointmentEntity.TimeStart);
                }
            }

            return appointmentStartTimesList;
        }

        public static List<TimeOnly> GetAppointmentEndTimesFromEntityList(List<Appointment> appointmentsList, int doctorId)
        {
            /* Get the appointment end times from an appointment entity list */

            List<TimeOnly> appointmentEndTimesList = new List<TimeOnly>();

            foreach (Appointment appointmentEntity in appointmentsList)
            {
                if (doctorId == -1)
                {
                    appointmentEndTimesList.Add(appointmentEntity.TimeEnd);
                }
                else if (doctorId == appointmentEntity.Doctor.Id)
                {
                    appointmentEndTimesList.Add(appointmentEntity.TimeEnd);
                }
            }

            return appointmentEndTimesList;
        }

        public static List<int>? GetPatientIdsFromEntityList(List<Appointment> appointmentsList, int doctorId)
        {
            /* Get the patient ids times from an appointment entity list */

            List<int> patientIdList = new List<int>();

            foreach (Appointment appointmentEntity in appointmentsList)
            {
                if (doctorId == -1)
                {
                    patientIdList.Add(appointmentEntity.Patient.Id);
                }
                else if (doctorId == appointmentEntity.Doctor.Id)
                {
                    patientIdList.Add(appointmentEntity.Patient.Id);
                }
            }

            return patientIdList;
        }
    }
}
