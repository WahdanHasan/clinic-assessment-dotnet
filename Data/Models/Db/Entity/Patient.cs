using clinic_assessment_redone.Data.Models.Db.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Entity
{
    [Table("patient")]
    public class Patient : ClinicUser
    {

        [Column("blood_type")]
        public string bloodType { get; set; }

        public List<Appointment> patientAppointments { get; set; }

    }
}
