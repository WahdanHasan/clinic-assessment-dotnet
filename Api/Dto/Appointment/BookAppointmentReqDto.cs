namespace clinic_assessment_redone.Api.Dto.Appointment
{
    public class BookAppointmentReqDto
    {
        public int patientId { get; set; }
        public int doctorId { get; set; }
        public string appointmentDate { get; set; }
        public string appointmentTimeStart { get; set; }
        public string appointmentTimeEnd { get; set; }
    }
}
