using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Entity
{
    [Table("employee")]
    public class Employee : ClinicUser
    {

        [Column("phone_ext")]
        public string PhoneExt { get; set; }

        [Column("office_room_num")]
        public string OfficeRoomNumber { get; set; }

        [Column("work_start_times")]
        public List<TimeOnly> WorkStartTimes { get; set; }

        [Column("work_end_times")]
        public List<TimeOnly> WorkEndTimes { get; set; }
    }
}
