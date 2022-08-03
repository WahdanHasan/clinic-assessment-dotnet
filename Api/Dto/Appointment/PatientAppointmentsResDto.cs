namespace clinic_assessment_redone.Api.Dto.Appointment
{
    public class PatientAppointmentsResDto
    {
        public int patientId { get; set; }
        public string patientFirstName { get; set; }
        public string patientLastName { get; set; }

        public List<AppointmentDetailsResDto> appointments { get; set; }
    }
}
