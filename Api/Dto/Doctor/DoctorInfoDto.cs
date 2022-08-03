using clinic_assessment_redone.Helpers.Converter;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace clinic_assessment_redone.Api.Dto.Doctor
{
    public class DoctorInfoDto
    {
        public int id { get; set; } 
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string nationality { get; set; }

        [property: JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly dateOfBirth { get; set; }
        public string gender { get; set; }
        public string phoneNumber { get; set; }
        public string phoneExtension { get; set; }
        public string officeRoomNumber { get; set; }
        public List<string> workSchedule { get; set; }

        [JsonIgnore]
        [property: JsonConverter(typeof(TimeOnlyListConverter))]
        public List<TimeOnly> workStartTimes { get; set; }

        [JsonIgnore]
        [property: JsonConverter(typeof(TimeOnlyListConverter))]
        public List<TimeOnly> workEndTimes { get; set; }
        public string specialty { get; set; }
    }
}
