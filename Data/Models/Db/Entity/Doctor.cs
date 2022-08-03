using clinic_assessment_redone.Data.Models.Db.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Entity
{

    [Table("doctor")]
    public class Doctor : Employee
    {
        [Column("specialty")]
        public string specialty { get; set; }

        public List<Appointment> doctorAppointments { get; set; }

    }
}
