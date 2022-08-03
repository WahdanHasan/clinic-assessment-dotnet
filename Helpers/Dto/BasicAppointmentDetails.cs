
using System.Text.Json.Serialization;

namespace clinic_assessment_redone.Helpers.Dto
{
    public class BasicAppointmentDetails
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? patientId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string doctorFullName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string patientFullName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string date { get; set; }
        public string timeStart { get; set; }
        public string timeEnd { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string durationMins { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? patientAttended { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string appointmentStatus { get; set; }
    }
}
