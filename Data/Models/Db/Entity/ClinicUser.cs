using clinic_assessment.data.Models.Db.Repo;
using clinic_assessment_redone.Helpers.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace clinic_assessment.data.Models.Db.Entity
{
    [Table("clinic_user")]
    public class ClinicUser
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [property: JsonConverter(typeof(DateOnlyConverter))]
        [Column("date_of_birth", TypeName = "date")]
        public DateOnly DateOfBirth { get; set; }

        [Column("nationality")]
        public string Nationality { get; set; }

        [Column("gender")]
        public string Gender { get; set; }

        [Column("phone_country_code")]
        public string PhoneCountryCode { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        public List<UserRoles> UserRoles { get; set; }
    }
}
