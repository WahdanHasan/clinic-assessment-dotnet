namespace clinic_assessment_redone.Api.Dto.Doctor
{
    public class BusyDoctorByAptCountResDto
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int appointmentCount { get; set; }
    }
}
