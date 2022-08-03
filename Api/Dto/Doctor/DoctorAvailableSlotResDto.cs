using clinic_assessment_redone.Helpers.Dto;

namespace clinic_assessment_redone.Api.Dto.Doctor
{
    public class DoctorAvailableSlotResDto
    {
        public string doctorFullName { get; set; }                   
        public int doctorId { get; set; }
        public List<BasicAppointmentDetails> availableSlots { get; set; }
        public List<BasicAppointmentDetails> occupiedSlots { get; set; }
    }
}
