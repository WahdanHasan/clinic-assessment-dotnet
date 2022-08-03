namespace clinic_assessment_redone.Helpers.Dto
{
    public class GetDoctorResDtoDto
    {
        public string phoneExt { get; set; }

        public string officeRoomNo { get; set; }

        public List<string> workStartTimes { get; set; }

        public List<string> workEndTimes { get; set; }

        public string specialty { get; set; }
    }
}
