using clinic_assessment_redone.Helpers.Converter;
using System.Text.Json.Serialization;

namespace clinic_assessment_redone.Api.Dto.Appointment
{
    public class AppointmentDetailsResDto
    {
        public int doctorId { get; set; }
        public string doctorFullName { get; set; }

        [property: JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly date { get; set; }

        [property: JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly timeStart { get; set; }

        [property: JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly timeEnd { get; set; }
        public int durationMins { get; set; }
        public bool patientAttended { get; set; }
        public string status { get; set; }
    }
}
