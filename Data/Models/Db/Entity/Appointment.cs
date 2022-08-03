using clinic_assessment.data.Models.Db.Entity;
using clinic_assessment_redone.Helpers.Converter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clinic_assessment_redone.Data.Models.Db.Entity
{
    [Table("appointment")]
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        //[ForeignKey("doctor_id")]
        [Column("doctor_id")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        //[ForeignKey("patient_id")]
        [Column("patient_id")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [Column("date_created", TypeName ="date")]
        [property: JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly DateCreated { get; set; }

        [property: JsonConverter(typeof(TimeOnlyConverter))]
        [Column("time_start")]
        public TimeOnly TimeStart { get; set; }

        [property: JsonConverter(typeof(TimeOnlyConverter))]
        [Column("time_end")]
        public TimeOnly TimeEnd { get; set; }

        [Column("duration_mins")]
        public int DurationMins { get; set; }

        [Column("patient_attended")]
        public bool PatientAttended { get; set; }

        [Column("status")]
        public string Status { get; set; }
    }
}
